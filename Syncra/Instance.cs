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
    private Dictionary<Type, Dictionary<Guid, Node>> Nodes { get; }
    private Task UpdateTask { get; }
    
    public Instance(Guid guid, bool localInstance = false)
    {
        World = World.Create();
        Nodes = new Dictionary<Type, Dictionary<Guid, Node>>();
        Uuid = guid;
        
        // debug
        var spinnerNode = new SpinnerNode(World);
        AddNode(spinnerNode);
        spinnerNode.RotationSpeed = new RotationSpeed(new Vector3(1.0f, 1.0f, 1.0f));
        
        UpdateTask = new Task(Update);
        UpdateTask.Start();
    }
    
    private void AddNode(Node node)
    {
        var nodeType = node.GetType();
        if (!Nodes.ContainsKey(nodeType))
        {
            Nodes[nodeType] = new Dictionary<Guid, Node>();
        }
        Nodes[nodeType].Add(node.Uuid.Value, node);
    }
    
    public void Update()
    {
        while (true)
        {
            // run input systems in sequential-parallel
            
            // run core systems in sequential-parallel
            if (Nodes.TryGetValue(typeof(SpinnerNode), out var spinnerNodes))
            {
                Parallel.ForEach(spinnerNodes.Values, node =>
                {
                    node.Update();
                });
            }
            
            // run user scripts in parallel
            
            // run render systems in sequential-parallel

            Thread.Sleep(100); // debug
        }
    }
}
