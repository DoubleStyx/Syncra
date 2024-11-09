using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public class SpinnerSystem : IWorldSystem
{
    public void Run(Instance instance, double delta)
    {
        var queryDescription = new QueryDescription().WithAll<TransformComponent, SpinnerComponent>();

        instance.World.Query(in queryDescription, (ref TransformComponent transform, ref SpinnerComponent spinner) =>
        {
            var rotationDelta = spinner.RotationSpeed.ToQuaternion(); 
            transform.Rotation = Quaternion.Normalize(transform.Rotation * rotationDelta);
        });
    }
}