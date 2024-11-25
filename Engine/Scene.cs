namespace Syncra.Engine;

public sealed class Scene
{
	public EcsContext Context;
	public Guid Guid;
	public JobSystem JobSystem;
	public string Name;

	internal Scene()
	{
		Name = "";
		Guid = new Guid();
		Context = new EcsContext();
		JobSystem = new JobSystem();
	}
}