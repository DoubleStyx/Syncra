namespace Syncra.SyncraEngine;

public interface IComponent
{
    /// Hardcoded dependencies specific to the component type
    protected abstract IReadOnlyList<Guid> Dependencies { get; }

    /// Runtime dependencies specific to the instance
    protected abstract List<Guid> customDependencies { get; }

    /// Add a runtime dependency
    public void AddCustomDependency(Guid dependency)
    {
        if (!customDependencies.Contains(dependency))
        {
            customDependencies.Add(dependency);
        }
    }

    /// Remove a runtime dependency
    public void RemoveCustomDependency(Guid dependency)
    {
        customDependencies.Remove(dependency);
    }

    /// Unified access to both hardcoded and custom dependencies
    public IEnumerable<Guid> GetAllDependencies()
    {
        return Dependencies.Concat(customDependencies);
    }

    public virtual void Initialize()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Cleanup()
    {

    }
}