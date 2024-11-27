using System.Collections.Concurrent;
using System.Net;

namespace Syncra.SyncraEngine;

public sealed class World
{
    private Guid SessionGuid;
    private Guid WorldGuid;
    private string SessionName;
    private string WorldName;
    private IPAddress IpAddress;
    private int Port;
    private JobScheduler JobScheduler;
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>> _components;

    internal World(IPAddress ip, int port, Guid sessionGuid, Guid worldGuid)
    {

    }

    public IEnumerable<IComponent> GetAllComponents()
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
}