namespace Syncra;

public class Engine
{
    private List<Instance> Instances = new List<Instance>();
    private Instance CurrentInstance;

    public Engine()
    {
        CreateInstance(defaultInstance: true);
    }

    private void ChangeCurrentInstance(Instance currentInstance)
    {
        CurrentInstance = currentInstance;
    }

    private void CreateInstance(bool defaultInstance = false)
    {
        Instance instance = new Instance(defaultInstance);
        Instances.Add(instance);
        if (defaultInstance)
            CurrentInstance = instance;
        instance.Run();
    }

    private void DestroyInstance(Instance instance)
    {
        Instances.Remove(instance);
    }
}