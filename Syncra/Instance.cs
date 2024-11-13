using System.Numerics;
using Arch.Core;
using Syncra.Components;
using Syncra.Systems;
using Guid = System.Guid;

namespace Syncra;

public class Instance
{
    public World World { get; }
    public Guid Guid { get; }
    private Task UpdateTask { get; }
    private DateTime UpdateStartTime { get; set; }
    private TimeSpan TickInterval { get; set; }
    
    public Instance(bool localInstance = false)
    {
        World = World.Create();
        Guid = Guid.NewGuid();
        TickInterval = TimeSpan.FromMilliseconds(100);
        
        // debug
        for (int i = 0; i < 10; i++)
            Archetypes.SpinnerNode.New(World);
        
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
            
            // run core system 1 in parallel
            SpinnerSystem.Update(World);
            
            // run user scripts for after1 hook in parallel using updateOrder for passes
            
            // run core system 2 in parallel
            
            // run user scripts for after2 hook in parallel using updateOrder for passes
            
            // etc...
            
            // push to async render buffer
            
            // submit changesets

            // throttle with update rate
            var frameTime = DateTime.Now - UpdateStartTime;
            if (frameTime < TickInterval)
            Thread.Sleep(TickInterval - frameTime);
        }
    }
}
