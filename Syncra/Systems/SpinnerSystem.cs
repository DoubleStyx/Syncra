using System.Numerics;
using Arch.Core;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Systems;

public class SpinnerSystem : ISystem
{
    public void Run(Instance instance)
    {
        var queryDescription = new QueryDescription().WithAll<SpinnerSystem>();

        instance.World.Query(in queryDescription, (ref Syncra.Components.SpinnerNode spinner) =>
        {
            var rotationDelta = spinner.RotationSpeed.ToQuaternion();
            Node node = spinner.Node;
            node.transform.Rotation = Quaternion.Normalize(node.transform.Rotation * rotationDelta);
            
            // temporary debug
            Program.Logger?.Log(LogLevel.Info, $"Spinner rotation: {node.transform.Rotation}");
            
            spinner.Node = node;
        });
    }
}