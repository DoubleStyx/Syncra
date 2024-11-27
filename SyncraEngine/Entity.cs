namespace Syncra.SyncraEngine;

/// A simple handle to an entity
public struct Entity
{
    internal readonly Guid Guid;
    private readonly World _world;

    internal Entity(World world)
    {
        Guid = Guid.NewGuid();
        _world = world;
    }

    public readonly void AddComponent<T>() where T : IComponent, new()
    {
        _world.AddComponent<T>(this);
    }

    public readonly T? TryGetComponent<T>() where T : IComponent
    {
        _world.TryGetComponent<T>(this, out var component);
        return component;
    }

    public readonly void DestroyComponent<T>() where T : IComponent
    {
        _world.DestroyComponent<T>(this);
    }
}