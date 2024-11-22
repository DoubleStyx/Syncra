namespace SyncraEngine;

public sealed class JobSystem
{
    private readonly List<ISystem> _systems = [];
    private Dictionary<ISystem, List<ISystem>>? _cachedDependencyGraph;
    private List<List<ISystem>>? _cachedSortedSystems;

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);
        InvalidateCache();
    }

    public void RemoveSystem(ISystem system)
    {
        _systems.Remove(system);
        InvalidateCache();
    }

    private void InvalidateCache()
    {
        _cachedDependencyGraph = null;
        _cachedSortedSystems = null;
    }
    
    private void DetectCycle(Dictionary<ISystem, List<ISystem>> graph)
    {
        var visited = new HashSet<ISystem>();
        var stack = new HashSet<ISystem>();

        foreach (var system in graph.Keys)
        {
            Visit(system);
        }

        return;

        void Visit(ISystem system)
        {
            if (stack.Contains(system))
                throw new InvalidOperationException("Cycle detected in the system dependency graph.");

            if (visited.Contains(system))
                return;

            stack.Add(system);
            foreach (var dependency in graph[system])
            {
                Visit(dependency);
            }

            stack.Remove(system);
            visited.Add(system);
        }
    }


    private Dictionary<ISystem, List<ISystem>> BuildDependencyGraph()
    {
        if (_cachedDependencyGraph != null)
            return _cachedDependencyGraph;

        var graph = new Dictionary<ISystem, List<ISystem>>();

        foreach (var system in _systems)
        {
            graph[system] = [];
            foreach (var dependency in system.Dependencies
                         .Select(dependencyType => _systems.FirstOrDefault(s => s.GetType() == dependencyType))
                         .OfType<ISystem>()) graph[system].Add(dependency);
        }

        _cachedDependencyGraph = graph;
        return graph;
    }

    private IEnumerable<List<ISystem>> TopologicalSort(Dictionary<ISystem, List<ISystem>> graph)
    {
        if (_cachedSortedSystems != null)
            return _cachedSortedSystems;

        var inDegree = new Dictionary<ISystem, int>();
        foreach (var system in graph)
        {
            inDegree[system.Key] = 0;
        }

        foreach (var dependency in from edges in graph.Values
                 from dependency in edges
                 where inDegree.ContainsKey(dependency)
                 select dependency)
        {
            inDegree[dependency]++;
        }

        var zeroInDegree =
            new Queue<ISystem>(inDegree.Where(static kvp => kvp.Value == 0).Select(static kvp => kvp.Key));
        var sortedBatches = new List<List<ISystem>>();

        while (zeroInDegree.Count > 0)
        {
            var batch = new List<ISystem>();
            foreach (var system in zeroInDegree.ToList())
            {
                batch.Add(system);
                zeroInDegree.Dequeue();

                foreach (var dependent in graph[system])
                {
                    inDegree[dependent]--;
                    if (inDegree[dependent] == 0) zeroInDegree.Enqueue(dependent);
                }
            }

            sortedBatches.Add(batch);
        }

        _cachedSortedSystems = sortedBatches;
        return sortedBatches;
    }

    public async Task Execute(Scene scene)
    {
        var graph = BuildDependencyGraph();
        DetectCycle(graph);
        var sortedSystems = TopologicalSort(graph);

        var tasks = sortedSystems.Select(batch =>
            Task.WhenAll(batch.Select(system =>
                Task.Run(() => system.Update(scene))))).ToList();

        await Task.WhenAll(tasks);
    }
}