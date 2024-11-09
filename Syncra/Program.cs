using System;
using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Handles;
using Syncra.Systems;

namespace Syncra;

public class Program
{
    static void Main(string[] args)
    {
        var vrContext = new VRContext();

        var world = World.Create();

        var entity = world.Create(new TransformComponent
            {
                Position = new Vector3(0, 0, 0),
                Rotation = Quaternion.Identity,
                Scale = Vector3.One
            },
            new SpinnerComponent { RotationSpeed = new Vector3(0.1f, 0.2f, 0.3f) });

        var spinnerSystem = new SpinnerSystem();

        while (true)
        {
            spinnerSystem.Run(world);
            var queryDescription = new QueryDescription().WithAll<TransformComponent>();

            world.Query<TransformComponent>(in queryDescription, (ref TransformComponent transform) =>
            {
                Console.WriteLine($"Transform.Rotation: {transform.Rotation}");
                
                float magnitude = MathF.Sqrt(transform.Rotation.X * transform.Rotation.X +
                                             transform.Rotation.Y * transform.Rotation.Y +
                                             transform.Rotation.Z * transform.Rotation.Z +
                                             transform.Rotation.W * transform.Rotation.W);
                Console.WriteLine($"Quaternion Magnitude: {magnitude}");

            });
            System.Threading.Thread.Sleep(100);
        }

        world.Dispose();
    }
}