using System.Collections.Concurrent;
using System.Numerics;

namespace Syncra.SyncraEngine;

public class World
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>> _components = new();
    private readonly List<object> _systems = new();
    public float deltaTime;

    public T AddComponent<T>(Entity entity)
    {
        var comp = default(T);
        _components.GetOrAdd(typeof(T), static _ => new ConcurrentDictionary<Guid, object>()).TryAdd(entity.Guid, comp);
        return comp;
    }

    public T? GetComponent<T>(Entity entity)
    {
        if (!_components.TryGetValue(typeof(T), out var componentDict)) return default;

        componentDict.TryGetValue(entity.Guid, out var component);
        return (T?)component;
    }

    public void RemoveComponent<T>(Entity entity)
    {
        if (!_components.TryGetValue(typeof(T), out var componentDict)) return;

        componentDict.TryRemove(entity.Guid, out _);
    }
}