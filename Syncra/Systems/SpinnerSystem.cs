using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public class SpinnerSystem
{
    public void Run(World world)
    {
        var queryDescription = new QueryDescription().WithAll<TransformComponent, SpinnerComponent>();

        world.Query<TransformComponent, SpinnerComponent>(in queryDescription, (ref TransformComponent transform, ref SpinnerComponent spinner) =>
        {
            Quaternion rotationDelta = spinner.RotationSpeed.ToQuaternion(); 
            transform.Rotation = Quaternion.Normalize(transform.Rotation * rotationDelta);
        });
    }
}