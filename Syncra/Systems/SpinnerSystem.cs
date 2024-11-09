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
            
            /*
            Console.WriteLine($"Transform.Rotation: {transform.Rotation}");
                
            float magnitude = MathF.Sqrt(transform.Rotation.X * transform.Rotation.X +
                                         transform.Rotation.Y * transform.Rotation.Y +
                                         transform.Rotation.Z * transform.Rotation.Z +
                                         transform.Rotation.W * transform.Rotation.W);
            Console.WriteLine($"Quaternion Magnitude: {magnitude}");
            */
        });
    }
}