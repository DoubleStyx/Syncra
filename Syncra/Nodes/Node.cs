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
        get => Entity.Get<Name>();
        set => Entity.Set(value);
    }
    public Uuid Uuid
    {
        get => Entity.Get<Uuid>();
        set => Entity.Set(value);
    }
    public Active Active
    {
        get => Entity.Get<Active>();
        set => Entity.Set(value);
    }
    public Transform Transform
    {
        get => Entity.Get<Transform>();
        set => Entity.Set(value);
    }
    public Parent Parent
    {
        get => Entity.Get<Parent>();
        set => Entity.Set(value);
    }
    public Children Children
    {
        get => Entity.Get<Children>();
        set => Entity.Set(value);
    }


    public Node(World world)
    {
        Entity = world.Create(new Name(), new Uuid(), new Active(), new Transform(), new Parent(), new Children());
    }
    
    public virtual void Update()
    {
        
    }
}