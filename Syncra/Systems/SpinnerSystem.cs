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
        world.Query(in query, (Entity entity, ref Components.Guid guid, ref LocalTransform transform, ref RotationSpeed rotationSpeed) =>
        {
            components.Add((guid, transform, rotationSpeed));
        });

        // Process components in parallel
        Parallel.ForEach(components, (item) =>
        {
            var (guid, transform, rotationSpeed) = item;

            rotationSpeed.value.Y = (float)System.Math.Sin(DateTime.Now.Ticks + guid.value.GetHashCode());

            transform.rotation = Quaternion.Normalize(transform.rotation * rotationSpeed.value.ToQuaternion());

            // temporary debug
            Program.Logger?.Log(LogLevel.Info, $"Guid: {guid.value} Spinner rotation: {transform.rotation} RotationSpeed: {rotationSpeed.value}");
        });
    }
}