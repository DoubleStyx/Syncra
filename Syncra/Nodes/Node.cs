using Syncra.Components;

namespace Syncra.Nodes;

public class Node
{
    public readonly Entity Entity;
    public Name Name
    {
        get => Entity.Get<Name>();
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
    public Instance Instance
    {
        get => Entity.Get<Instance>();
        set => Entity.Set(value);
    }

    public System.Guid Guid
    {
        get => Entity.Get<System.Guid>();
    }
    
    public Node(Syncra.Instance instance)
    {
        Entity = new Entity(instance);
        // technically there's no protection against adding unauthorized components within-class,
        // but it's difficult to chain together component additions without it
        // The number of archetype changes is equal to the number of inheritance levels
        
        // We need to figure out how to batch structural changes together in a chained manner somehow
        
        // since structs are immutable, you usually reinstantiate to modify
        Entity.Add(new Name());
        Entity.Add(new Active());
        Entity.Add(new Transform());
        Entity.Add(new Parent());
        Entity.Add(new Children());
        Entity.Add(new Components.Instance(instance));
        Entity.Add(new Components.Guid());
    }
    
    public virtual void Update()
    {
        
    }
}