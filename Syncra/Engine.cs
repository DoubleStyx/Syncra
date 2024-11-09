namespace Syncra;

public class Engine
{
    private List<Instance> Instances = new List<Instance>();
    private Instance CurrentInstance;

    public Engine()
    {
        CreateInstance(defaultInstance: true);
    }

    public void Run()
    {
        while (true)
        {
            foreach (var instance in Instances)
            {
                instance.Update();
            }

            Thread.Sleep(100);
        }
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
            ChangeCurrentInstance(instance);
    }

    private void DestroyInstance(Instance instance)
    {
        Instances.Remove(instance);
    }
}