namespace Syncra.SyncraEngine;

/// The base interface for all components
public interface IComponent
{
    public abstract Guid Guid { get; }
    public abstract IReadOnlyList<Guid> Dependencies { get; }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
}