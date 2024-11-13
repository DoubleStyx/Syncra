using Arch.Core;
using Arch.Core.Extensions;

namespace Syncra;

public class Entity
{
    private readonly Arch.Core.Entity Value;

    public Entity(Instance instance)
    {
        Value = instance.World.Create();
    }

    public void Add<T>(T component)
    {
        Value.Add(component);
    }

    public T Get<T>()
    {
        return Value.Get<T>();
    }

    public void Set<T>(T component)
    {
        Value.Set(component);

        var instance = Get<Components.Instance>().Value;
        var guid = Get<Components.Guid>().Value;

        if (!instance.DirtyComponents.ContainsKey(guid))
        {
            instance.DirtyComponents[guid] = new List<Type>();
        }

        if (!instance.DirtyComponents[guid].Contains(typeof(T)))
        {
            instance.DirtyComponents[guid].Add(typeof(T));
        }
    }
}