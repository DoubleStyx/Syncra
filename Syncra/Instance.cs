using Arch.Core;
using Syncra.Systems;

namespace Syncra;

public class Instance
{
    public World World = World.Create();
    public Dictionary<ulong, Entity> EntityMap = new();
    public List<IWorldSystem> WorldSystems = new();
    public double Time;

    public Instance()
    {
        WorldSystems.AddRange([
            new SpinnerSystem(),
        ]);
    }
    public void Update(double delta)
    {
        Time += delta;
        foreach (var system in WorldSystems) system.Run(this, delta);
    }
}
