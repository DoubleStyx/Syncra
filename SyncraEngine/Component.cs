namespace Syncra.SyncraEngine;

public abstract class Component
{
    public List<Component> Dependencies { get; }

    public abstract void Update();
}