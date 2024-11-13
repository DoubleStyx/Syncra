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
    private DateTime UpdateStartTime { get; set; }
    private TimeSpan TickInterval { get; set; }
    
    public Instance(bool localInstance = false)
    {
        World = World.Create();
        Guid = Guid.NewGuid();
        Nodes = new Dictionary<Type, Dictionary<Guid, Node>>();
        DirtyComponents = new Dictionary<Guid, List<Type>>();
        TickInterval = TimeSpan.FromMilliseconds(100);
        
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
            UpdateStartTime = DateTime.Now;
            
            // process incoming changesets
            
            // fetch async input buffer
            
            // run core system 1 in parallel
            
            // run user scripts for after1 hook in parallel using updateOrder for passes
            
            // run core system 2 in parallel
            
            // run user scripts for after2 hook in parallel using updateOrder for passes
            
            // etc...
            
            if (Nodes.TryGetValue(typeof(SpinnerNode), out var spinnerNodes))
            {
                Parallel.ForEach(spinnerNodes.Values, node =>
                {
                    node.Update();
                });
            }
            
            // push to async render buffer
            
            // submit changesets

            // throttle with update rate
            var frameTime = DateTime.Now - UpdateStartTime;
            if (frameTime < TickInterval)
            Thread.Sleep(TickInterval - frameTime);
        }
    }
}
