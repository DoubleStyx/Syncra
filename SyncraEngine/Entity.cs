namespace Syncra.SyncraEngine;

public struct Entity
{
    internal Guid Guid;
    private readonly Scene _scene;

    internal Entity(Scene scene)
    {
        Guid = Guid.NewGuid();
        _scene = scene;
    }

    public readonly void AddComponent<T>() where T : Component, new()
    {
        _scene.AddComponent<T>(this);
    }

    public readonly T? TryGetComponent<T>() where T : Component
    {
        _scene.TryGetComponent<T>(this, out var component);
        return component;
    }

    public readonly void DestroyComponent<T>() where T : Component
    {
        _scene.DestroyComponent<T>(this);
    }
}