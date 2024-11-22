namespace SyncraEngine;

public class World
{
    public List<Scene> Scenes;

    public World()
    {
        // Scene container
        Scenes = new List<Scene>();
        // Add default scene
        Scenes.Add(new Scene());
    }
}