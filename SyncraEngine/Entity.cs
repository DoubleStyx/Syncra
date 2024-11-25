namespace Syncra.SyncraEngine;

public struct Entity
{
    internal Guid Guid;
    private readonly EcsContext _context;

    internal Entity(EcsContext context)
    {
        Guid = Guid.NewGuid();
        _context = context;
    }

    public readonly void AddComponent<T>() where T : IComponent, new()
    {
        _context.AddComponent<T>(this);
    }

    public readonly T? TryGetComponent<T>() where T : IComponent
    {
        _context.TryGetComponent<T>(this, out var component);
        return component;
    }

    public readonly void DestroyComponent<T>() where T : IComponent
    {
        _context.DestroyComponent<T>(this);
    }
}