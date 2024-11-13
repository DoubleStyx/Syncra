using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using NLog;
using Syncra.Components;
using Syncra.Math;

namespace Syncra.Nodes;

public struct SpinnerNode : Node
{
    
    public SpinnerNode(Instance instance)
    {
        Entity.Add(new RotationSpeed());
    }
    
    public override void Update()
    {
        Transform.Rotation = Quaternion.Normalize(Transform.Rotation * RotationSpeed.Value.ToQuaternion());
    
        // temporary debug
        Program.Logger?.Log(LogLevel.Info, $"Spinner rotation: {Transform.Rotation} RotationSpeed: {RotationSpeed.Value}");
        

    }
}
