using System.Collections.Concurrent;

namespace Syncra.SyncraEngine;

public sealed class World
{
    private Guid Guid;
    private string Name;
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Component>> _components;

    internal World(Guid worldGuid = new Guid(), string ipAddress = null)
    {
        if (worldGuid != null)
        {
            if (ipAddress != null)
            {
                // attempt to connect to another user
            }
            else
            {
                // attempt to load world from asset server
            }
        }
        else
        {
            // new world
        }
    }

    private IEnumerable<Component> GetAllComponents()
    {
        foreach (var typeComponents in _components.Values)
        foreach (var component in typeComponents.Values)
            yield return component;
    }

    public Entity CreateEntity()
    {
        return new Entity(this);
    }

    public IEnumerable<(Guid Entity, T Component)> GetAllComponents<T>() where T : Component
    {
        if (!_components.TryGetValue(typeof(T), out var components)) yield break;

        foreach (var kvp in components)
            yield return (kvp.Key, (T)kvp.Value);
    }

    internal void AddComponent<T>(Entity entity) where T : Component, new()
    {
        var components = _components.GetOrAdd(typeof(T), static _ => new ConcurrentDictionary<Guid, Component>());
        components[entity.Guid] = new T();
    }

    internal bool TryGetComponent<T>(Entity entity, out T? component) where T : Component
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

    internal void DestroyComponent<T>(Entity entity) where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var components)) components.TryRemove(entity.Guid, out _);
    }

    public async Task ExecuteAsync()
    {
        var components = GetAllComponents().ToList();

        BuildDependencyGraphs(components, out var graph, out var reverseGraph);

        var batches = TopologicalSortIntoBatches(graph, reverseGraph);

        foreach (var batch in batches)
        {
            var tasks = batch.Select(component => Task.Run(() => component.Update()));
            await Task.WhenAll(tasks);
        }
    }

    private static void BuildDependencyGraphs(
        IEnumerable<Component> components,
        out Dictionary<Component, List<Component>> graph,
        out Dictionary<Component, List<Component>> reverseGraph)
    {
        graph = new Dictionary<Component, List<Component>>();
        reverseGraph = new Dictionary<Component, List<Component>>();

        var componentsSet = new HashSet<Component>(components);

        foreach (var component in components)
        {
            graph[component] = component.Dependencies.Where(dep => componentsSet.Contains(dep)).ToList();
            reverseGraph[component] = new List<Component>();
        }

        foreach (var component in components)
        foreach (var dependency in graph[component])
            reverseGraph[dependency].Add(component);
    }

    private static List<List<Component>> TopologicalSortIntoBatches(
        Dictionary<Component, List<Component>> graph,
        Dictionary<Component, List<Component>> reverseGraph)
    {
        var inDegree = graph.ToDictionary(static kvp => kvp.Key, static kvp => kvp.Value.Count);
        var batches = new List<List<Component>>();
        var zeroInDegree = new Queue<Component>(inDegree.Where(static kvp => kvp.Value == 0).Select(static kvp => kvp.Key));

        while (zeroInDegree.Count > 0)
        {
            var batch = new List<Component>();
            var batchSize = zeroInDegree.Count;

            for (var i = 0; i < batchSize; i++)
            {
                var component = zeroInDegree.Dequeue();
                batch.Add(component);

                foreach (var dependent in reverseGraph[component])
                {
                    inDegree[dependent]--;
                    if (inDegree[dependent] == 0) zeroInDegree.Enqueue(dependent);
                }
            }

            batches.Add(batch);
        }

        if (inDegree.Any(static kvp => kvp.Value > 0))
            throw new InvalidOperationException("Cycle detected in component dependencies.");

        return batches;
    }
}