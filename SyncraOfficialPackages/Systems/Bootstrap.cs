using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Bootstrap : ISystem
{
    public List<ISystem> Dependencies { get; }

    public void Update(Scene scene, Guid entity)
    {
        
    }
}