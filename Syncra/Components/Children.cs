namespace Syncra.Components;

public struct Children : IComponent
{
    public List<Entity> Value;
}

public class ChildrenSystem
{
    public void Update(Scene scene)
    {
        foreach (Entity entity in scene.Entities)
        {
            
        }
    }
}