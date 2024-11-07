namespace Components;

public struct Parent : Component
{
    public Entity Value;
}

public class ParentSystem : System
{
    public override void Update(Scene.Scene scene)
    {
        foreach (var entity in scene.Entities)
        {
            if (entity.Has<Parent>() && entity.Has<Transform>())
            {
                var parentComponent = entity.Get<Parent>();
                var parentTransform = parentComponent.Value.Get<Transform>();
                var childTransform = entity.Get<Transform>();

                childTransform.Position += parentTransform.Position;

                entity.AddComponent(childTransform);
            }
        }
    }
}