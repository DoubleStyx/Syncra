namespace Syncra.SyncraEngine;

public class Entity
{
    public Guid Guid;
    public World World;

    public Entity(Guid guid, World world)
    {
        Guid = guid;
        World = world;
    }

    public T AddComponent<T>()
    {
        return World.AddComponent<T>(this);
    }

    public T? GetComponent<T>()
    {
        return World.GetComponent<T>(this);
    }

    public void RemoveComponent<T>()
    {
        World.RemoveComponent<T>(this);
    }
}