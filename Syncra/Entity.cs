using System.Collections.Generic;

public class Entity
{
    private static int nextId = 0;
    public int Id { get; }
    
    private Dictionary<System.Type, Component> components = new Dictionary<System.Type, Component>();
    
    public Entity() => Id = nextId++;

    public void AddComponent<T>(T component) where T : Component
    {
        components[typeof(T)] = component;
    }

    public bool Has<T>() where T : Component => components.ContainsKey(typeof(T));

    public T Get<T>() where T : Component => (T)components[typeof(T]);
}
