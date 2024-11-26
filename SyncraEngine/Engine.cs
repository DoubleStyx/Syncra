namespace Syncra.SyncraEngine;

public sealed class Engine
{
    public Guid Guid = new();
    public string Name = "";
    public World CurrentWorld = new();

    public List<World> Worlds =
    [
        new()
    ];
}