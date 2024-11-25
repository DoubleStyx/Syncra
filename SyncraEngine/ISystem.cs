namespace Syncra.SyncraEngine;

public interface ISystem
{
	public List<Type> Dependencies { get; }
	public List<Type> Signature { get; }

	public void Update(Scene scene)
	{
	}
}