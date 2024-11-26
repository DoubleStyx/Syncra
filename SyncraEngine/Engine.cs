namespace Syncra.SyncraEngine;

public sealed class Engine
{
    public World? CurrentWorld = null;
    public List<World> Worlds = [];

    internal Engine()
    {
        CurrentWorld = new World();
        Worlds.Add(CurrentWorld);
    }
}