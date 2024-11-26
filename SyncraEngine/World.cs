namespace Syncra.SyncraEngine;

public sealed class World
{
    private readonly List<Scene> _scenes = [];
    public Guid Guid = new();
    public string Name = "";
    private readonly Scene? _currentScene;

    internal World(Guid worldGuid = new Guid(), string ipAddress = null)
    {
        if (worldGuid != null)
        {
            if (ipAddress != null)
            {
                // attempt to connect to another user
            }
            else
            {
                // attempt to load world from asset server
            }
        }
        else
        {
            // new scene
            _currentScene = new Scene();
            _scenes.Add(_currentScene);
        }
    }
}