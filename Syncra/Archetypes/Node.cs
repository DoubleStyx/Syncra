using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Guid = Syncra.Components.Guid;

namespace Syncra.Archetypes;

public class Node
{
    public static Entity New(World world)
    {
        Entity entity = world.Create();
        
        entity.Add(
            new Name(),
            new Active(),
            new LocalTransform(),
            new GlobalTransform(),
            new Parent(),
            new Children(),
            new Guid(),
            new Components.Archetype(typeof(Node))
            );

        return entity;
    }
}