using System.Numerics;
using NLog;
using Syncra.Components;
using Syncra.Math;
using Arch.Core.Extensions;
using Syncra.Types;

namespace Syncra.Nodes;

public class SpinnerNode : Node
{
    public RotationSpeed RotationSpeed
    {
        get => Entity.Get<RotationSpeed>();
        set => Entity.Set(value);
    }

    public SpinnerNode(Instance instance) : base(instance)
    {
        Entity.Add(new RotationSpeed());
    }
    
    public override void Update()
    {
        Transform = new Transform(Transform.Position, 
            Quaternion.Normalize(Transform.Rotation * RotationSpeed.Value.ToQuaternion()),
            Transform.Scale);
        
        // temporary debug
        Program.Logger?.Log(LogLevel.Info, $"Spinner rotation: {Transform.Rotation}");
    }
}
