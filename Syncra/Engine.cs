using Syncra.Drivers;

namespace Syncra;

/// <summary>
/// The owner of all worlds for the application. We probably won't make this a singleton to keep things flexible.
/// The local scene might need to be put on a Task since the window event loop will consume the main thread.
/// </summary>
public class Engine
{
    /// <summary>
    /// All the worlds managed by the engine.
    /// </summary>
    public readonly Dictionary<Guid, World> Worlds;
    
    /// <summary>
    /// The local scene, which has privileged permissions.
    /// </summary>
    public readonly Scene LocalScene;

    /// <summary>
    /// Creates a new engine instance.
    /// </summary>
    public Engine()
    {
        Worlds = new Dictionary<Guid, World>();
        LocalScene = new Scene(new Guid());
    }

    /// <summary>
    /// Starts the main engine update loop.
    /// </summary>
    public void Run()
    {
        /*
        // run local home in here to keep the main thread busy
        while (true)
        {
            LocalScene.Update();
            Thread.Sleep(100);
        }
        */

        Window window = new Window();
        window.Run();
    }
}