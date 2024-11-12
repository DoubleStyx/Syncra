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
        set
        {
            Entity.Set(value);
        }
    }
    public Uuid Uuid
    {
        get
        {
            return Entity.Get<Uuid>();
        }
        set
        {
            Entity.Set(value);
        }
    }
    public Active Active
    {
        get
        {
            return Entity.Get<Active>();
        }
        set
        {
            Entity.Set(value);
        }
    }
    public Transform Transform
    {
        get
        {
            return Entity.Get<Transform>();
        }
        set
        {
            Entity.Set(value);
        }
    }
    public Parent Parent
    {
        get
        {
            return Entity.Get<Parent>();
        }
        set
        {
            Entity.Set(value);
        }
    }
    public Children Children
    {
        get
        {
            return Entity.Get<Children>();
        }
        set
        {
            Entity.Set(value);
        }
    }

    public Node(World world)
    {
        Entity = world.Create(new Name(), new Uuid(), new Active(), new Transform(), new Parent(), new Children());
    }
    
    public virtual void Update()
    {
        
    }
}