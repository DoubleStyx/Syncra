using Arch.Core;
using Syncra.Systems;

namespace Syncra;

public class Instance
{
    private World World = World.Create();
    private Dictionary<ulong, Entity> EntityMap = new();
    private List<IWorldSystem> Systems = new();
    private DateTime StartTime = DateTime.Now;
    private bool networked = true;
    private DateTime lastUpdate = DateTime.Now - TimeSpan.FromMilliseconds(100);
    private Thread worldThread;
    
    public Instance(bool defaultInstance = false)
    {
        if (defaultInstance)
        {
            // default instance initialization
            networked = false;
        }
        else
        {
            // networked instance initialization
        }
    }

    public void Run()
    {
        Thread thread = new(UpdateLoop);
        worldThread = thread;
    }

    private void UpdateLoop()
    {
        while (true)
        {
            Update();
            Thread.Sleep(100);
        }
    }
    
    private void Update()
    {
        double delta = (DateTime.Now - lastUpdate).TotalNanoseconds / 1000000000;
        foreach (var system in Systems) system.Run(this, delta);
    }
}
