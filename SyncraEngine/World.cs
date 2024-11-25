namespace Syncra.Engine;

public sealed class World
{
	private readonly List<Scene> _scenes =
	[
		new()
	];

	public Guid Guid = new();
	public string Name = "";
}