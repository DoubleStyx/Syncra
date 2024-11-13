using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Schedulers;
using Syncra.Components;
using Syncra.Systems;
using Guid = System.Guid;

namespace Syncra;

public class Instance
{
    public World World { get; }
    public Dictionary<Guid, Entity> EntityMap { get; }
    public Guid Guid { get; }
    
    public DateTime UpdateStartTime { get; set; }
    
    public TimeSpan TickInterval { get; set; }
    
    public Instance(bool localInstance = false)
    {
        TickInterval = TimeSpan.FromMilliseconds(100);
        World = World.Create();
        Guid = Guid.NewGuid();
        EntityMap = new Dictionary<Guid, Entity>();
        
        World.SharedJobScheduler = new(
            new JobScheduler.Config
            {
                ThreadPrefixName = "Syncra",
                ThreadCount = 8,
                MaxExpectedConcurrentJobs = 64,
                StrictAllocationMode = false,
            }
        );
        
        // debug start
        for (int i = 0; i < 1; i++)
            Archetypes.SpinnerNode.New(this);
        // debug end
    }
}
