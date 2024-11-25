namespace Syncra.SyncraEngine;

public sealed class Engine
{
    public Guid Guid = new();
    public string Name = "";

    public List<World> Worlds =
    [
        new()
    ];
}