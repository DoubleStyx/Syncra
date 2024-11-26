namespace Syncra.SyncraEngine;

public struct Entity
{
    internal Guid Guid;
    private readonly World _world;

    internal Entity(World world)
    {
        Guid = Guid.NewGuid();
        _world = world;
    }

    public readonly void AddComponent<T>() where T : Component, new()
    {
        _world.AddComponent<T>(this);
    }

    public readonly T? TryGetComponent<T>() where T : Component
    {
        _world.TryGetComponent<T>(this, out var component);
        return component;
    }

    public readonly void DestroyComponent<T>() where T : Component
    {
        _world.DestroyComponent<T>(this);
    }
}