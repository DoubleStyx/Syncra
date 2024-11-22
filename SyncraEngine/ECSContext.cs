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

    public T GetComponent<T>(Guid entity) where T : IComponent, new()
    {
        Components.TryGetValue(typeof(T), out var components);

        if (components == null)
        {
            return new T();
        }

        if (components.TryGetValue(entity, out var component))
        {
            return (T)component;
        }

        return new T();
    }


    public void DestroyComponent<T>(Guid entity) where T : IComponent, new()
    {
        Components.TryGetValue(typeof(T), out var components);

        if (components == null)
        {
            return;
        }

        if (components.TryGetValue(entity, out var component))
        {
            components.Remove(entity);
            return;
        }
    }
}