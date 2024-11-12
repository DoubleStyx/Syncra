using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Syncra.Types;

namespace Syncra;

public class Node
{
    public Entity Entity { get; }

    public Name Name
    {
        get
        {
            return Entity.Get<Name>();
        }
    }
    public UUID UUID
    {
        get
        {
            return Entity.Get<UUID>();
        }
    }
    public Active Active
    {
        get
        {
            return Entity.Get<Active>();
        }
    }
    public Transform Transform
    {
        get
        {
            return Entity.Get<Transform>();
        }
    }
    public Parent Parent
    {
        get
        {
            return Entity.Get<Parent>();
        }
    }
    public Children Children
    {
        get
        {
            return Entity.Get<Children>();
        }
    }

    public Node()
    {
        Entity = new Entity();
        Entity.Add<Name, UUID, Active, Transform, Parent, Children>();
    }
    
    public virtual void Update()
    {
        
    }
}