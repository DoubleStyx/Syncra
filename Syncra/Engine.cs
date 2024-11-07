using System.Collections.Generic;
using System.Threading;


public class Engine
{
    public List<Scene.Scene> Scenes = new List<Scene.Scene>();
    public Scene.Scene ActiveScene = null;

    public void Start()
    {
        if (ActiveScene == null && Scenes.Count > 0)
            ActiveScene = Scenes[0];

        while (true)
        {
            if (ActiveScene != null)
            {
                ActiveScene.Update();
            }

            Thread.Sleep(16);
        }
    }
}