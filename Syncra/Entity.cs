using Arch.Core;
using Arch.Core.Extensions;

namespace Syncra;

public class Entity
{
    public readonly Guid Guid;
    private readonly Arch.Core.Entity Value;
    private readonly Instance Instance;

    public Entity(Instance instance)
    {
        Value = instance.World.Create();
        Instance = instance;
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

        if (!Instance.DirtyComponents.ContainsKey(Guid))
        {
            Instance.DirtyComponents[Guid] = new List<Type>();
        }

        if (!Instance.DirtyComponents[Guid].Contains(typeof(T)))
        {
            Instance.DirtyComponents[Guid].Add(typeof(T));
        }
    }
}