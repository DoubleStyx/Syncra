using Syncra.SyncraEngine;
using SyncraCorePackages.Components;

namespace SyncraCorePackages.Systems;

public class VelocitySystem
{
    public List<Guid> Dependencies => [Guid.Parse("151958129517")];

    public void Update(World world, Entity entity, Transform transform, Velocity velocity)
    {
        transform.Position += velocity.PositionSpeed * world.deltaTime;
        transform.Rotation += velocity.RotationSpeed * world.deltaTime;
    }
}