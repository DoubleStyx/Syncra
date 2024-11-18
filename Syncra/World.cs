namespace Syncra;

public class World
{
    public readonly Dictionary<Guid, Scene> Scenes;
    public Guid CurrentScene;
    public Task WorldTask;

    public World(Guid guid, bool local = false)
    {
        Scenes = new Dictionary<Guid, Scene>();
        if (local == true)
        {
            var scene = new Scene(new Guid(), true);
            Scenes.Add(scene);
        }
        CurrentScene
    }
}