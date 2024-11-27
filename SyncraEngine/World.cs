using System.Collections.Concurrent;

namespace Syncra.SyncraEngine;

public sealed class World
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>> _components;

    internal World()
    {
        _components = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>>();
    }

    private IEnumerable<IComponent> GetAllComponents()
    {
        foreach (var typeComponents in _components.Values)
        foreach (var component in typeComponents.Values)
            yield return component;
    }

    public Entity CreateEntity()
    {
        return new Entity(this);
    }

    public IEnumerable<(Guid Entity, T Component)> GetAllComponents<T>() where T : IComponent
    {
        if (!_components.TryGetValue(typeof(T), out var components)) yield break;

        foreach (var kvp in components)
            yield return (kvp.Key, (T)kvp.Value);
    }

    internal void AddComponent<T>(Entity entity) where T : IComponent, new()
    {
        var components = _components.GetOrAdd(typeof(T), static _ => new ConcurrentDictionary<Guid, IComponent>());
        components[entity.Guid] = new T();
    }

    internal bool TryGetComponent<T>(Entity entity, out T? component) where T : IComponent
    {
        if (_components.TryGetValue(typeof(T), out var components) &&
            components.TryGetValue(entity.Guid, out var comp))
        {
            component = (T)comp;
            return true;
        }

        component = default;
        return false;
    }

    internal void DestroyComponent<T>(Entity entity) where T : IComponent
    {
        if (_components.TryGetValue(typeof(T), out var components)) components.TryRemove(entity.Guid, out _);
    }

    public async Task ExecuteAsync()
    {
        var IComponents = World.GetAllComponents().ToList();

        BuildDependencyGraphs(IComponents, out var graph, out var reverseGraph);

        var batches = TopologicalSortIntoBatches(graph, reverseGraph);

        foreach (var batch in batches)
        {
            var tasks = batch.Select(IComponent => Task.Run(() => IComponent.Update()));
            await Task.WhenAll(tasks);
        }
    }

    private static void BuildDependencyGraphs(
        IEnumerable<IComponent> IComponents,
        out Dictionary<IComponent, List<IComponent>> graph,
        out Dictionary<IComponent, List<IComponent>> reverseGraph)
    {
        graph = new Dictionary<IComponent, List<IComponent>>();
        reverseGraph = new Dictionary<IComponent, List<IComponent>>();

        var ComponentsSet = new HashSet<IComponent>(IComponents);

        foreach (var IComponent in IComponents)
        {
            graph[IComponent] = IComponent.Dependencies;
            reverseGraph[IComponent] = new List<IComponent>();
        }

        foreach (var IComponent in IComponents)
        foreach (var dependency in graph[IComponent])
            reverseGraph[dependency].Add(IComponent);
    }

    private static List<List<IComponent>> TopologicalSortIntoBatches(
        Dictionary<IComponent, List<IComponent>> graph,
        Dictionary<IComponent, List<IComponent>> reverseGraph)
    {
        var inDegree = graph.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value.Count);
        var batches = new List<List<IComponent>>();
        var zeroInDegree = new Queue<IComponent>(inDegree.Where(static kvp => kvp.Value == 0).Select(static kvp => kvp.Key));

        while (zeroInDegree.Count > 0)
        {
            var batch = new List<IComponent>();
            var batchSize = zeroInDegree.Count;

            for (var i = 0; i < batchSize; i++)
            {
                var IComponent = zeroInDegree.Dequeue();
                batch.Add(IComponent);

                foreach (var dependent in reverseGraph[IComponent])
                {
                    inDegree[dependent]--;
                    if (inDegree[dependent] == 0) zeroInDegree.Enqueue(dependent);
                }
            }

            batches.Add(batch);
        }

        if (inDegree.Any(static kvp => kvp.Value > 0))
            throw new InvalidOperationException("Cycle detected in IComponent dependencies.");

        return batches;
    }
}