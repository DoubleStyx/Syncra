namespace SyncraEngine;

public class World
{
    public string Name;
    public Guid Guid;
    public List<Scene> Scenes;

    public World()
    {
        // Scene container
        Scenes = new List<Scene>();
        // Add default scene
        Scenes.Add(new Scene());
    }
}