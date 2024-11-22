using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Bootstrap : ISystem
{
    public List<Type> Dependencies { get; } = new();
    public List<Type> Signature { get; }

    public void Update(Scene scene, Guid entity)
    {
    }
}