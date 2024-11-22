namespace SyncraEngine;

public class Scene
{
    public string Name;
    public Guid Guid;
    public ECSContext Context;
    public JobSystem JobSystem;

    public Scene()
    {
        // ECS context
        Context = new ECSContext();
        // Job system
        JobSystem = new JobSystem();
    }
}