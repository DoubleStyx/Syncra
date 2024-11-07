using System.Collections.Generic;
using System.Threading;

namespace Syncra;

public class Engine
{
    public List<Scene> Scenes = new List<Scene>();
    public Scene ActiveScene = null;

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