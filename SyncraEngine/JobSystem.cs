namespace SyncraEngine;

public sealed class JobSystem
{
    public List<ISystem> Systems = [];
    public List<Task> Tasks = [];

    public void OnSystemUpdated()
    {
        // check systems for dependency resolution

        // job scheduler basically just tries to execute the DAG as fast as possible
        // and then start over again
    }
}