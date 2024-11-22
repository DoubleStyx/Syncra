using System.Collections.Concurrent;

namespace SyncraEngine;

public sealed class EcsContext
{
    public readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>> Components;

    internal EcsContext()
    {
        Components = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>>();
    }

    public void AddComponent<T>(Entity entity) where T : IComponent, new()
    {
        var components = Components.GetOrAdd(typeof(T), static _ => new ConcurrentDictionary<Guid, IComponent>());
        components[entity.Guid] = new T();
    }

    public T? GetComponent<T>(Entity entity) where T : IComponent
    {
        if (Components.TryGetValue(typeof(T), out var components) 
            && components.TryGetValue(entity.Guid, out var component)) 
            return (T)component;

        return default;
    }

    public void DestroyComponent<T>(Entity entity) where T : IComponent
    {
        if (Components.TryGetValue(typeof(T), out var components)) components.TryRemove(entity.Guid, out _);
    }
}