using Arch.Core;

namespace Syncra.Systems;

public interface IWorldSystem
{
    void Run(Instance instance, double delta);
}
