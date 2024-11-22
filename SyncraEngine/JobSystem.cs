namespace SyncraEngine;

public sealed class JobSystem
{
    private List<ISystem> Systems { get; } = new();

    public async Task Execute(Scene scene)
    {
        var dependencyGraph = BuildDependencyGraph();

        var sortedSystems = TopologicalSort(dependencyGraph);

        var tasks = sortedSystems.Select(batch => Task.WhenAll(batch.Select(system => Task.Run(() => system.Update(scene))))).ToList();

        await Task.WhenAll(tasks);
    }

    private Dictionary<ISystem, List<ISystem>> BuildDependencyGraph()
    {
        var graph = new Dictionary<ISystem, List<ISystem>>();

        foreach (var system in Systems)
        {
            graph[system] = new List<ISystem>();
            foreach (var dependency in system.Dependencies.Select(dependencyType => Systems.FirstOrDefault(static s => false)).OfType<ISystem>())
            {
                graph[system].Add(dependency);
            }
        }

        return graph;
    }

    private static IEnumerable<List<ISystem>> TopologicalSort(Dictionary<ISystem, List<ISystem>> graph)
    {
        var inDegree = new Dictionary<ISystem, int>();
        foreach (var system in graph)
        {
            inDegree[system.Key] = 0;
        }

        foreach (var dependency in from edges in graph.Values from dependency in edges where inDegree.ContainsKey(dependency) select dependency)
        {
            inDegree[dependency]++;
        }

        var zeroInDegree = new Queue<ISystem>(inDegree.Where(static kvp => kvp.Value == 0).Select(static kvp => kvp.Key));
        var sortedBatches = new List<List<ISystem>>();

        while (zeroInDegree.Count != 0)
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

        return sortedBatches;
    }
}