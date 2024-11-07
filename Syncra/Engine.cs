namespace Syncra;

public class Engine
{
    public List<Scene> Scenes = new List<Scene>();
    public Scene ActiveScene = null;

    public void SetActiveScene(Scene scene)
    {
        ActiveScene = scene;
    }

    public void Start()
    {
        if (ActiveScene == null && Scenes.Count > 0)
            ActiveScene = Scenes[0];

        while (true)
        {
            ActiveScene?.Update();
            Thread.Sleep(16);
        }
    }
}