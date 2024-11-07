using Syncra.Components;

namespace Syncra;
public class Scene
{
    public List<Entity> Entities = new List<Entity>();
    public List<ISystem> Systems = new List<ISystem>();
    public Dictionary<uint, MeshRenderer> meshRenderers = new Dictionary<uint, MeshRenderer>();
    public Dictionary<uint, Transform> transforms = new Dictionary<uint, Transform>();
    

    public Entity CreateEntity()
    {
        var entity = new Entity();
        Entities.Add(entity);
        return entity;
    }

    public void AddSystem(ISystem system)
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