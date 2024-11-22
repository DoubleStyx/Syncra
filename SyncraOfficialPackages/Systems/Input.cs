using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Input : ISystem
{
    public List<Type> Dependencies { get; } = new();
    public List<Type> Signature { get; }
}