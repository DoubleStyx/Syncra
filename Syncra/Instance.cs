using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Systems;

namespace Syncra;

public class Instance
{
    public World World = World.Create();
    private Dictionary<ulong, Entity> EntityMap = new();
    private List<IWorldSystem> Systems = new();
    private DateTime StartTime = DateTime.Now;
    private bool networked = true;
    private DateTime lastUpdate = DateTime.Now - TimeSpan.FromMilliseconds(100);
    
    public Instance(bool defaultInstance = false)
    {
        if (defaultInstance)
        {
            // default instance initialization
            networked = false;
            var entity = World.Create(new TransformComponent
                {
                    Position = new Vector3(0, 0, 0),
                    Rotation = Quaternion.Identity,
                    Scale = Vector3.One
                },
                new SpinnerComponent { RotationSpeed = new Vector3(0.1f, 0.2f, 0.3f) });
            Systems.Add(new SpinnerSystem());
        }
        else
        {
            // networked instance initialization
        }
    }
    
    public void Update()
    {
        double delta = (DateTime.Now - lastUpdate).TotalNanoseconds / 1000000000;
        foreach (var system in Systems) system.Run(this, delta);
    }
}
