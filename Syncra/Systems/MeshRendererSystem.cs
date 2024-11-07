using Leopotam.Ecs;
using Syncra.Components;

namespace Syncra.Systems;

public class MeshRendererSystem : IEcsRunSystem
{
    private readonly EcsFilter<MeshRendererComponent> _filter = null;

    public void Run()
    {
        foreach (var i in _filter)
        {
            
        }
    }
}