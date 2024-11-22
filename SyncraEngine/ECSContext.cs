namespace SyncraEngine;

public class ECSContext
{
    private Dictionary<Type, Dictionary<Guid, IComponent>> Components;
    
    public ECSContext()
    {
        Components = new Dictionary<Type, Dictionary<Guid, IComponent>>();
    }

    public void AddComponent<T>(Guid entity) where T : IComponent, new()
    {
        Components.TryGetValue(typeof(T), out var components);
        components ??= new Dictionary<Guid, IComponent>();
        components.Add(entity, new T());
    }

    public T GetComponent<T>(Guid entity)
    {
        Components.TryGetValue(typeof(T), out var components);
        components ??= new Dictionary<Guid, IComponent>();
        return components[entity];
    }

    public void DestroyComponent(Guid entity, IComponent component)
    {
        
    }
}