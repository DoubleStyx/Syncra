using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Syncra.Components;
using Syncra.Systems;
using Guid = System.Guid;

namespace Syncra;

public class Instance
{
    public World World { get; }
    public Dictionary<Guid, Entity> EntityMap { get; } = new();
    public Guid Guid { get; }
    private Task UpdateTask { get; }
    private DateTime UpdateStartTime { get; set; }
    private TimeSpan LastUpdateTime { get; set; }
    private TimeSpan TickInterval { get; set; }
    
    public Instance(bool localInstance = false)
    {
        World = World.Create();
        Guid = Guid.NewGuid();
        TickInterval = TimeSpan.FromMilliseconds(100);
        
        // debug start
        Entity spinnerNode = Archetypes.SpinnerNode.New(this);
        Entity childNode = Archetypes.Node.New(this);
        spinnerNode.Get<Name>().value = "SpinnerNode";
        childNode.Get<Name>().value = "ChildNode";
        childNode.Get<Parent>().value = childNode.Get<Components.Guid>().value;
        childNode.Get<LocalTransform>().value = Matrix4x4.CreateTranslation(new Vector3(1, 1, 1));
        // debug end
        
        UpdateTask = new Task(Update);
        UpdateTask.Start();
    }
    
    public void Update()
    {
        while (true)
        {
            UpdateStartTime = DateTime.Now;
            
            // process incoming changesets
            
            // fetch async input buffer
            
            // run core system 1
            //ParentSystem.Update(this);
            
            // run user scripts for after1 hook in parallel using updateOrder for passes
            
            // run core system 2
            SpinnerSystem.Update(this);
            
            // run user scripts for after2 hook in parallel using updateOrder for passes
            
            // etc...
            
            // push to async render buffer
            
            // submit changesets

            // throttle with update rate
            var frameTime = DateTime.Now - UpdateStartTime;
            LastUpdateTime = frameTime;
            if (frameTime < TickInterval)
            Thread.Sleep(TickInterval - frameTime);
        }
    }
}
