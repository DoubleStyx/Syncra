using System.Collections.Concurrent;
using System.Numerics;
using Arch.Core;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public static class SpinnerSystem
{
    public static void Update(World world)
    {
        var query = new QueryDescription().WithAll<Components.Guid, LocalTransform, RotationSpeed>();
        var components = new List<(Components.Guid, LocalTransform, RotationSpeed)>();

        // Collect components
        world.Query(in query, (Entity entity, ref Components.Guid guid, ref LocalTransform localTransform, ref RotationSpeed rotationSpeed) =>
        {
            components.Add((guid, localTransform, rotationSpeed));
        });

        // Process components in parallel
        Parallel.ForEach(components, (item) =>
        {
            var (guid, localTransform, rotationSpeed) = item;

            rotationSpeed.value.Y = (float)System.Math.Sin(DateTime.Now.Ticks + guid.value.GetHashCode());

            localTransform.rotation = Quaternion.Normalize(localTransform.rotation * rotationSpeed.value.ToQuaternion());

            // temporary debug
            Program.Logger?.Log(LogLevel.Info, $"Guid: {guid.value} Spinner rotation: {localTransform.rotation} RotationSpeed: {rotationSpeed.value}");
        });
    }
}