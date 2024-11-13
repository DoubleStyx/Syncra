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
        var query = new QueryDescription().WithAll<Components.Guid, LocalTransform, GlobalTransform, Parent>();
        var components = new List<(Components.Guid, LocalTransform, GlobalTransform, Parent)>();

        world.Query(in query, (Entity entity, ref Components.Guid guid, ref LocalTransform transform, ref GlobalTransform globalTransform, ref Parent parent) =>
        {
            components.Add((guid, transform, globalTransform, parent));
        });

        Parallel.ForEach(components, (item) =>
        {
            var (guid, localTransform, globalTransform, parent) = item;
            
            
        });
    }
}