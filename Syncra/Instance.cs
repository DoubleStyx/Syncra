using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Arch.Core;
using Syncra.Components;
using Syncra.Nodes;
using Guid = System.Guid;

namespace Syncra;

public class Instance
{
    public World World { get; }
    public Guid Guid { get; }
    private Dictionary<Type, Dictionary<Guid, Node>> Nodes { get; }
    public Dictionary<Guid, List<Type>> DirtyComponents { get; } 
    private Task UpdateTask { get; }
    
    public Instance(bool localInstance = false)
    {
        World = World.Create();
        Guid = Guid.NewGuid();
        Nodes = new Dictionary<Type, Dictionary<Guid, Node>>();
        DirtyComponents = new Dictionary<Guid, List<Type>>();
        
        // debug
        var spinnerNode = new SpinnerNode(this);
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
        Nodes[nodeType].Add(node.Entity.Get<Components.Guid>().Value, node);
    }
    
    public void Update()
    {
        while (true)
        {
            // run input systems in sequential-parallel
            
            // process incoming changesets
            
            // run core systems in sequential-parallel
            if (Nodes.TryGetValue(typeof(SpinnerNode), out var spinnerNodes))
            {
                Parallel.ForEach(spinnerNodes.Values, node =>
                {
                    node.Update();
                });
            }
            
            // run user scripts in parallel
            
            // submit changesets
            
            // run render systems in sequential-parallel

            Thread.Sleep(100); // debug
        }
    }
}
