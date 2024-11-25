#region

using System.Collections.Concurrent;

#endregion

namespace Syncra.SyncraEngine;

public sealed class EcsContext
{
	private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>> _components;

	internal EcsContext()
	{
		_components = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, IComponent>>();
	}

	public Entity CreateEntity()
	{
		return new Entity(this);
	}

	public IEnumerable<(Guid Entity, T Component)> GetAllComponents<T>() where T : IComponent
	{
		if (!_components.TryGetValue(typeof(T), out var components)) yield break;

		foreach (var kvp in components)
			yield return (kvp.Key, (T)kvp.Value);
	}

	internal void AddComponent<T>(Entity entity) where T : IComponent, new()
	{
		var components = _components.GetOrAdd(typeof(T), static _ => new ConcurrentDictionary<Guid, IComponent>());
		components[entity.Guid] = new T();
	}

	internal bool TryGetComponent<T>(Entity entity, out T? component) where T : IComponent
	{
		if (_components.TryGetValue(typeof(T), out var components) &&
		    components.TryGetValue(entity.Guid, out var comp))
		{
			component = (T)comp;
			return true;
		}

		component = default;
		return false;
	}

	internal void DestroyComponent<T>(Entity entity) where T : IComponent
	{
		if (_components.TryGetValue(typeof(T), out var components)) components.TryRemove(entity.Guid, out _);
	}
}