using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Syncra.Types;

namespace Syncra;

public class Node
{
    protected readonly Syncra.Entity Entity;
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
    
    public Node(Instance instance)
    {
        Entity = new Entity(instance);
        // technically there's no protection against adding unauthorized components within-class,
        // but it's difficult to chain together component additions without it
        // The number of archetype changes is equal to the number of inheritance levels
        Entity.Add(new Name(), new Uuid(), new Active(), new Transform(), new Parent(), new Children());
    }
    
    public virtual void Update()
    {
        
    }
}