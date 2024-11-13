using System.Collections.Concurrent;
using System.Numerics;
using Arch.Core;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public static class SpinnerSystem
{
    public static void Update(Instance instance)
    {
        var world = instance.World;
        var query = new QueryDescription().WithAll<Name, LocalTransform, RotationSpeed>();
        var components = new List<(Name, LocalTransform, RotationSpeed)>();

        world.Query(in query, (Entity entity, ref Name name, ref LocalTransform localTransform, ref RotationSpeed rotationSpeed) =>
        {
            components.Add((name, localTransform, rotationSpeed));
        });

        Parallel.ForEach(components, (item) =>
        {
            var (name, localTransform, rotationSpeed) = item;

            rotationSpeed.value.Y = 1f;
            
            Quaternion rotationQuaternion = rotationSpeed.value.ToQuaternion();

            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotationQuaternion);
            Program.Logger?.Debug($"Rotation matrix: {rotationMatrix}");
            
            localTransform.value = Matrix4x4.Multiply(localTransform.value, rotationMatrix);
            Program.Logger.Debug($"Local matrix: {localTransform.value}");
        });
    }
}