using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Guid = Syncra.Components.Guid;

namespace Syncra.Nodes;

public struct Node
{
    public readonly Entity Entity;
    
    public Node(Syncra.Instance instance)
    {
        Entity = instance.World.Create();
        // technically there's no protection against adding unauthorized components within-class,
        // but it's difficult to chain together component additions without it
        // The number of archetype changes is equal to the number of inheritance levels
        // We need to figure out how to batch structural changes together in a chained manner somehow
        Entity.Add(new Name(), new Active(), new Transform(), new Parent(), new Children(), new Components.Instance(instance), new Guid());
    }
    
    // needs to use core updateOrder
    public virtual void Update()
    {
        
    }
}