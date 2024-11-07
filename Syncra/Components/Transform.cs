namespace Components;

public struct Transform : Component
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

public class TransformSystem : System
{
    public override void Update(Scene.Scene scene)
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