using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Systems;

namespace Syncra;

public class Instance
{
    public World World = World.Create();
    public List<ISystem> Systems = new();
    public Dictionary<long, int> UUIDTable = new();
    public WorldAccessLevel WorldAccessLevel;
    public string IP;
    public int ID;
    public bool hidden;
    public DateTime StartTime;
    public DateTime LastUpdate;
    public double LastUpdateDelta;
    
    public Instance(bool defaultInstance = false, string IP = "127.0.0.1", WorldAccessLevel accessLevel = WorldAccessLevel.Private)
    {
        if (defaultInstance)
        {
            WorldAccessLevel = WorldAccessLevel.Private;
            var entity = World.Create(
                new SpinnerNode { RotationSpeed = new Vector3(0.1f, 0.2f, 0.3f) });
            Systems.Add(new SpinnerSystem());
        }
        else
        {
            // networked instance initialization
        }
    }
    
    public void Update()
    {
        foreach (var system in Systems) system.Run(this);
    }
}
