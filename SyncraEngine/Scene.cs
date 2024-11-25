namespace Syncra.SyncraEngine;

public sealed class Scene
{
	public EcsContext Context;
	public Guid Guid;
	public string Name;
	public JobSystem JobSystem;

	internal Scene()
	{
		Name = "";
		Guid = new Guid();
		Context = new EcsContext();
		JobSystem = new JobSystem();
	}
}