namespace Syncra;

public class Entity
{
    private static int nextId = 0;
    public int Id { get; }
    
    private Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();
    
    public Entity() => Id = nextId++;

    public void AddComponent<T>(T component) where T : IComponent
    {
        components[typeof(T)] = component;
    }

    public bool Has<T>() where T : IComponent => components.ContainsKey(typeof(T));

    public T Get<T>() where T : IComponent => (T)components[typeof(T)];
}
