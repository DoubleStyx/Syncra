using System.Numerics;

namespace Syncra.Components;

public struct RotationSpeed
{
    public Vector3 Value;

    public RotationSpeed()
    {
        Value = Vector3.Zero;
    }
    
    public RotationSpeed(Vector3 value)
    {
        Value = value;
    }
}