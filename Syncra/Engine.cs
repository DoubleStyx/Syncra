using Syncra.Systems;

namespace Syncra;

public class Engine
{
    private Dictionary<Guid, Instance> Instances { get; }
    private Guid LocalInstance { get; }
    private Guid CurrentInstance { get; set; }

    public Engine()
    {
        Instances = new Dictionary<Guid, Instance>();
        Instance localInstance = new Instance(localInstance: true);
        Instances.Add(localInstance.Guid, localInstance);
        ChangeCurrentInstance(localInstance.Guid);
    }

    public void Run()
    {
        while (true)
        {
            // Arch ECS does not allow running parallelQuery off main thread,
            // So I'll need to fix that at some point
            // We need full control over each thread later on
            foreach (var instance in Instances.Values)
            {
                // sleep to meet tick interval
                if (DateTime.Now - instance.UpdateStartTime < instance.TickInterval)
                    continue;
                
                // throttle secondary instances
                if (instance.Guid != CurrentInstance &&
                    DateTime.Now - instance.UpdateStartTime < TimeSpan.FromSeconds(1))
                    continue;
                    
                instance.UpdateStartTime = DateTime.Now;

                // process incoming changesets

                // fetch async input buffer

                // run core system 1 in parallel
                //ParentSystem.Update(this);

                // run user scripts for after1 hook in parallel using updateOrder for passes

                // run core system 2 in parallel
                SpinnerSystem.Update(instance);

                // run user scripts for after2 hook in parallel using updateOrder for passes

                // etc...

                // push to async render buffer

                // submit changesets
            }
        }
    }

    // we'll have to think about thread safety during instance switching
    // as well as how the renderer itself manages/shares resources
    // tbh I don't think we'll keep any GPU resources for non-active
    // instances loaded
    private void ChangeCurrentInstance(Guid guid)
    {
        Instance? instance = Instances[guid];
        if (instance != null)
            CurrentInstance = Instances[guid].Guid;
    }

    private void JoinInstance(Guid guid)
    {
        Instance? instance = Instances.TryGetValue(guid, out Instance localInstance) ? localInstance : null;
        if (instance == null)
            instance = new Instance();
        Instances.Add(instance.Guid, instance);
    }

    // needs to switch to the last used/created instance
    private void LeaveInstance(Guid guid)
    {
        Instance? instance = Instances[guid];
        if (instance != null)
        {
            Instances.Remove(instance.Guid);
        }
    }
}