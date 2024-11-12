using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Arch.Core;
using Syncra.Components;
using Syncra.Nodes;

namespace Syncra;

public class Instance
{
    private World World { get; }
    public Guid Uuid { get; }
    private Dictionary<Guid, Node> Nodes { get; }
    private Thread UpdateThread { get; }
    
    public Instance(Guid guid, bool localInstance = false)
    {
        World = World.Create();
        Nodes = new Dictionary<Guid, Node>();
        Uuid = guid;
        
        // debug
        var spinnerNode = new SpinnerNode(World);
        Nodes.Add(spinnerNode.Uuid.Value, spinnerNode);
        spinnerNode.RotationSpeed = new RotationSpeed(new Vector3(1.0f, 1.0f, 1.0f));
        
        UpdateThread = new Thread(Update);
        UpdateThread.Start();
    }
    
    public void Update()
    {
        while (true)
        {
            // run input systems in sequential-parallel
            
            // run core systems in sequential-parallel
            foreach (var node in Nodes.Values)
            {
                node.Update();
            }
            
            // run user scripts in parallel
            
            // run render systems in sequential-parallel

            Thread.Sleep(100); // debug
        }
    }
}
