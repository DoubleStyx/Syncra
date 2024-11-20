namespace Syncra;

/// <summary>
/// A unique world. A world contains one or more scenes.
/// </summary>
public class World
{
    /// <summary>
    /// The scenes in a particular world.
    /// </summary>
    public readonly Dictionary<Guid, Scene> Scenes;
    
    /// <summary>
    /// The currently focused scene. Used to determine thread priority and which scene to render.
    /// </summary>
    public Guid CurrentScene;

    /// <summary>
    /// Creates a new World instance.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="local"></param>
    public World(Guid guid, bool local = false)
    {
        Scenes = new Dictionary<Guid, Scene>();
        if (local)
        {
            var scene = new Scene(new Guid());
            Scenes.Add(scene.guid, scene);
        }
    }
}