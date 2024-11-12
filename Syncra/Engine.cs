using System.Numerics;

namespace Syncra;

public class Engine
{
    private Dictionary<Guid, Instance> Instances { get; }
    private Instance LocalInstance { get; }
    private Guid CurrentInstance { get; set; }

    public Engine()
    {
        Instances = new Dictionary<Guid, Instance>();
        Instance localInstance = new Instance(guid: new Guid(), localInstance: true);
        Instances.Add(localInstance.Uuid, localInstance);
        ChangeCurrentInstance(localInstance.Uuid);
    }

    private void ChangeCurrentInstance(Guid guid)
    {
        Instance instance = Instances[guid];
        if (instance != null)
            CurrentInstance = Instances[guid].Uuid;
    }

    private void JoinInstance(Guid guid)
    {
        Instance instance = Instances.TryGetValue(guid, out Instance localInstance) ? localInstance : null;
        if (instance == null)
            instance = new Instance(guid);
        Instances.Add(instance.Uuid, instance);
        
    }

    private void LeaveInstance(Guid guid)
    {
        Instance instance = Instances[guid];
        if (instance != null)
        {
            Instances.Remove(instance.Uuid);
        }
    }
}