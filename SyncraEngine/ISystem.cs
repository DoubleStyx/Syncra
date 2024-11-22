namespace SyncraEngine;

public interface ISystem
{
    public List<ISystem> Dependencies { get; }

    public void Update(Scene scene)
    {
    }
}