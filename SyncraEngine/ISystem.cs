namespace Syncra.SyncraEngine;

public interface ISystem
{
    public List<Type> Dependencies { get; set; }
    public void Start();
    public void Update();
}