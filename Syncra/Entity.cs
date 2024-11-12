using Arch.Core;
using Arch.Core.Extensions;

namespace Syncra;

public class Entity
{
    protected readonly Arch.Core.Entity Value;
    protected Instance Instance;
    protected Guid Guid;

    public Entity(Instance instance)
    {
        Value = instance.World.Create();
        Instance = instance;
        Guid = Guid.NewGuid();
    }

    public void Add(object component)
    {
        Value.Add(component);
    }

    public void Add(params object[] components)
    {
        Value.Add(components);
    }

    public T Get<T>()
    {
        return Value.Get<T>();
    }

    public void Set(object component)
    {
        Value.Set(component);
        var componentType = component.GetType();

        if (!Instance.DirtyComponents.ContainsKey(Guid))
        {
            Instance.DirtyComponents[Guid] = new List<Type>();
        }

        if (!Instance.DirtyComponents[Guid].Contains(componentType))
        {
            Instance.DirtyComponents[Guid].Add(componentType);
        }
    }
}