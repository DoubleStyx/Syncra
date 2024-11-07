using Leopotam.Ecs;
using Syncra.Components;

namespace Syncra.Systems;

public class TransformSystem : IEcsRunSystem
{
    private readonly EcsFilter<TransformComponent> _filter = null;

    public void Run()
    {
        foreach (var i in _filter)
        {
            
        }
    }
}