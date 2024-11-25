namespace Syncra.SyncraEngine;

public sealed class JobSystem
{
	private readonly Dictionary<Type, ISystem> _systemLookup = new();
	private Dictionary<ISystem, List<ISystem>>? _cachedDependencyGraph;
	private List<List<ISystem>>? _cachedSortedSystems;

	public void AddSystem(ISystem system)
	{
		_systemLookup[system.GetType()] = system;
		InvalidateCache();
	}

	public void RemoveSystem(ISystem system)
	{
		_systemLookup.Remove(system.GetType());
		InvalidateCache();
	}

	private void InvalidateCache()
	{
		_cachedDependencyGraph = null;
		_cachedSortedSystems = null;
	}

	private Dictionary<ISystem, List<ISystem>> BuildDependencyGraph()
	{
		if (_cachedDependencyGraph != null)
			return _cachedDependencyGraph;

		var graph = new Dictionary<ISystem, List<ISystem>>();

		foreach (var system in _systemLookup.Values)
		{
			var dependencies = new List<ISystem>();
			foreach (var dependencyType in system.Dependencies)
				if (_systemLookup.TryGetValue(dependencyType, out var dependency))
					dependencies.Add(dependency);

			graph[system] = dependencies;
		}

		_cachedDependencyGraph = graph;
		return graph;
	}

	private List<List<ISystem>> TopologicalSort(Dictionary<ISystem, List<ISystem>> graph)
	{
		if (_cachedSortedSystems != null)
			return _cachedSortedSystems;

		var inDegree = graph.ToDictionary(static kvp => kvp.Key, static _ => 0);

		foreach (var dependencies in graph.Values)
		foreach (var dependency in dependencies)
			if (inDegree.ContainsKey(dependency))
				inDegree[dependency]++;

		var zeroInDegree = new Queue<ISystem>(
			inDegree.Where(static kvp => kvp.Value == 0).Select(static kvp => kvp.Key)
		);
		var sortedBatches = new List<List<ISystem>>();
		var visitedCount = 0;

		while (zeroInDegree.Count > 0)
		{
			var batch = new List<ISystem>();
			var batchSize = zeroInDegree.Count;

			for (var i = 0; i < batchSize; i++)
			{
				var system = zeroInDegree.Dequeue();
				batch.Add(system);
				visitedCount++;

				foreach (var dependent in graph[system])
				{
					inDegree[dependent]--;
					if (inDegree[dependent] == 0) zeroInDegree.Enqueue(dependent);
				}
			}

			sortedBatches.Add(batch);
		}

		if (visitedCount != graph.Count)
			throw new InvalidOperationException("Cycle detected in the system dependency graph.");

		_cachedSortedSystems = sortedBatches;
		return sortedBatches;
	}

	public async Task Execute(Scene scene)
	{
		var graph = BuildDependencyGraph();
		var sortedSystems = TopologicalSort(graph);

		var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
		foreach (var batch in sortedSystems)
			await Task.Run(() => { Parallel.ForEach(batch, options, system => { system.Update(scene); }); });
		// TODO: switch to parallel/foreach
		// TODO: efficient querying through smallest intersections first
		// TODO: caching and other optimizations?
	}
}