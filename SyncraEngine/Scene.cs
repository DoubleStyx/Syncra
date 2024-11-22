namespace SyncraEngine;

public class Scene
{
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