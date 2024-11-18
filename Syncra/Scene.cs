namespace Syncra;

public class Scene
{
    public Dictionary<Type, Dictionary<Guid, Type>> Components;
    public List<ISystem> Systems;
    public Task SceneTask;

    public Scene(Guid guid, bool local = true)
    {
        Components = new Dictionary<Type, Dictionary<Guid, Type>>();
        if (!local)
        {
            SceneTask = Task.Run(Run);
        }
    }

    public void Update()
    {
        foreach (var system in Systems)
        {
            
        }
    }

    private void Run()
    {
        while (true)
        {
            this.Update();
            Thread.Sleep(100);
        }
    }
}