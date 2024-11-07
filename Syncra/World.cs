using System.Numerics;
using Syncra.Components;
using Syncra.Systems;

namespace Syncra;

using Leopotam.Ecs;

public class World
{
    private EcsWorld _world;
    private EcsSystems _systems;

    public void Initialize()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);

        _systems.Add(new TransformSystem());
        _systems.Add(new MeshRendererSystem());

        _systems.Init();

        var entity = _world.NewEntity();
        entity.Get<TransformComponent>().Position = Vector3.One;
    }

    public void Update()
    {
        _systems.Run();
    }

    public void Dispose()
    {
        _systems.Destroy();
        _world.Destroy();
    }
}
