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

        world.ParallelQuery(in query, (ref Name name, ref LocalTransform localTransform, ref RotationSpeed rotationSpeed) =>
        {
            rotationSpeed.value.Y = 0.1f;
            Quaternion rotationQuaternion = rotationSpeed.value.ToQuaternion();
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotationQuaternion);
            localTransform.value = Matrix4x4.Multiply(localTransform.value, rotationMatrix);
            
            Program.Logger?.Log(LogLevel.Info, $"Local transform: {localTransform.value}");
        });
    }
}