using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Guid = Syncra.Components.Guid;

namespace Syncra.Archetypes;

public class Node
{
    public static Entity New(Instance instance)
    {
        Entity entity = instance.World.Create();
        
        System.Guid guid = System.Guid.NewGuid();
        
        entity.Add(
            new Name(),
            new Active(),
            new LocalTransform(),
            new GlobalTransform(),
            new Parent(),
            new Children(),
            new Guid(guid),
            new Components.Archetype(typeof(Node))
            );
        
        instance.EntityMap.Add(guid, entity);

        return entity;
    }
}