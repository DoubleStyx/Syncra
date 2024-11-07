using Syncra;
using System.Numerics;

namespace Syncra.Components;

public struct Transform : IComponent
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}

public class TransformSystem : ISystem
{
    public void Update(Scene scene)
    {
        foreach (var entity in scene.Entities)
        {
            if (entity.Has<Transform>())
            {
                var transform = entity.Get<Transform>();
                transform.Position += new Vector3(0, 1, 0);
                entity.AddComponent(transform);
            }
        }
    }
}