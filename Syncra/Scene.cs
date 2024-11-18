namespace Syncra;

public class Scene
{
    /// <summary>
    /// The set of components that exist across all entities in the scene.
    /// </summary>
    public Dictionary<Type, Dictionary<Guid, Type>> Components;
    
    /// <summary>
    /// The systems that are active within the scene. 
    /// </summary>
    public List<ISystem> Systems;
    
    /// <summary>
    /// The current Task representing the scene update loop.
    /// </summary>
    public Task SceneTask;
    
    /// <summary>
    /// The GUID uniquely identifying the scene.
    /// </summary>
    public Guid guid;

    /// <summary>
    /// Creates a new scene for the given world.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="local"></param>
    public Scene(Guid guid, bool local = true)
    {
        Components = new Dictionary<Type, Dictionary<Guid, Type>>();
        if (!local) SceneTask = Task.Run(Run);
    }

    /// <summary>
    /// The main update method for the scene. This is normally ran automatically by the scene Task.
    /// </summary>
    public void Update()
    {
        foreach (var system in Systems)
        {
        }
    }

    /// <summary>
    /// An update loop that runs the scene update continuously. Usually used by the scene Task.
    /// </summary>
    private void Run()
    {
        while (true)
        {
            Update();
            Thread.Sleep(100);
        }
    }
}