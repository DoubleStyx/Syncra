using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Output : ISystem
{
    public List<ISystem> Dependencies { get; } = new();
}