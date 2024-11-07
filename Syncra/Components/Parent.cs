namespace Syncra.Components;

public struct Parent : IComponent
{
    public Entity Value;
}

public class ParentSystem : ISystem
{
    public void Update(Scene scene)
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