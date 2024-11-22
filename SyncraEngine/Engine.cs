namespace SyncraEngine;

internal sealed class Engine
{
    public List<IDriver> Drivers;
    public List<World> Worlds;

    internal Engine()
    {
        Drivers = [];
        Worlds =
        [
            new World()
        ];
    }
}