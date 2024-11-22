using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Input : ISystem
{
    public List<ISystem> Dependencies { get; } = new();
}