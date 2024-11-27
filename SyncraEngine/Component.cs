namespace Syncra.SyncraEngine;

public abstract class Component
{
    public List<Component> Dependencies { get; } = new();

    // Called when the component is initialized
    public virtual void Initialize() { }

    // Called every frame or tick
    public virtual void Update() { }

    // Called when the component is removed or the object is destroyed
    public virtual void Cleanup() { }

    // Add a dependency to this component
    public void AddDependency(Component dependency)
    {
        if (!Dependencies.Contains(dependency))
            Dependencies.Add(dependency);
    }
}