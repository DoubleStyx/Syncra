namespace Syncra;

public class Engine
{
    public readonly Dictionary<Guid, World> Worlds;
    public readonly Scene LocalScene;

    public Engine()
    {
        Worlds = new Dictionary<Guid, World>();
        LocalScene = new Scene(new Guid());
    }

    public void Run()
    {
        // run local home in here to keep the main thread busy
        while (true)
        {
            LocalScene.Update();
            Thread.Sleep(100);
        }
    }
}