using System.Collections.Concurrent;
using System.Numerics;
using Arch.Core;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public static class ParentSystem
{
    public static void Update(World world)
    {
        var query = new QueryDescription().WithAll<Components.Guid, LocalTransform, Parent>();
        var components = new List<(Components.Guid, LocalTransform, Parent)>();

        world.Query(in query, (Entity entity, ref Components.Guid guid, ref LocalTransform transform, ref Parent parent) =>
        {
            components.Add((guid, transform, parent));
        });

        Parallel.ForEach(components, (item) =>
        {
            var (guid, transform, parent) = item;
            
            
        });
    }
}