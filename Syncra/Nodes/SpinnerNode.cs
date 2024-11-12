using System.Numerics;
using NLog;
using Syncra.Components;
using Syncra.Math;
using Syncra.Types;
using Arch.Core.Extensions;
namespace Syncra.Nodes;

public class SpinnerNode : Node
{
    public RotationSpeed RotationSpeed
    {
        get
        {
            return Entity.Get<RotationSpeed>();
        }
    }

    public SpinnerNode() : base()
    {
        Entity.Add<RotationSpeed>();
    }
    
    public override void Update()
    {
        var rotationDelta = RotationSpeed.Value.ToQuaternion();
        Transform transform = Transform;
        transform.Rotation = Quaternion.Normalize(Transform.Rotation * rotationDelta);
    
        // temporary debug
        Program.Logger?.Log(LogLevel.Info, $"Spinner rotation: {Transform.Rotation}");
    
        Entity.Set<Transform>(transform);
    }
}
