using System.Collections.Generic;


public class Scene
{
    public List<Entity> Entities = new List<Entity>();
    public List<System> Systems = new List<System>();

    public Entity CreateEntity()
    {
        var entity = new Entity();
        Entities.Add(entity);
        return entity;
    }

    public void AddSystem(System system)
    {
        Systems.Add(system);
    }

    public void Update()
    {
        foreach (var system in Systems)
        {
            system.Update(this);
        }
    }
}