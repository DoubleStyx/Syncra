namespace Syncra.Components;

public struct Name : IComponent
{
    public string Value;
}

public class NameSystem
{
    public void Update(Scene scene)
    {
        foreach (Entity entity in scene.Entities)
        {
            
        }
    }
}