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

    private void LeaveInstance(Guid guid)
    {
        Instance? instance = Instances[guid];
        if (instance != null)
        {
            Instances.Remove(instance.Guid);
        }
    }
}