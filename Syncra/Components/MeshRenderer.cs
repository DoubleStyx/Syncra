using Syncra.Assets;

namespace Syncra.Components;
public struct MeshRenderer : IComponent
{
    public Mesh Mesh;
}

public class MeshRendererSystem : ISystem
{
    public void Update(Scene scene)
    {
        foreach (Entity entity in scene.Entities)
        {
            
        }
    }
}