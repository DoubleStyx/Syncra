using System.Collections.Concurrent;
using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public static class ParentSystem
{
    public static void Update(Instance instance)
    {
        var world = instance.World;
        var query = new QueryDescription().WithAll<Name, LocalTransform, GlobalTransform, Parent>();
        var components = new List<(Name, LocalTransform, GlobalTransform, Parent)>();

        world.Query(in query, (Entity entity, ref Name name, ref LocalTransform localTransform, ref GlobalTransform globalTransform, ref Parent parent) =>
        {
            components.Add((name, localTransform, globalTransform, parent));
        });

        Parallel.ForEach(components, (item) =>
        {
            var (name, localTransform, globalTransform, parent) = item;
            
            if (instance.EntityMap.TryGetValue(parent.value, out Entity parentEntity))
            {
                globalTransform.value = Matrix4x4.Multiply(localTransform.value, parentEntity.Get<GlobalTransform>().value);
            }

            // Temporary debug logging
            Program.Logger?.Log(LogLevel.Info, $"Name {name.value}: GlobalTransform updated to {globalTransform.value}");
        });
    }
}