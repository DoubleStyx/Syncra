using System.Numerics;

namespace Syncra.Components;

public struct RotationSpeed
{
    public Vector3 Value;

    public RotationSpeed()
    {
        Value = Vector3.Zero;
    }
    
    public RotationSpeed(float x, float y, float z)
    {
        Value = new Vector3(x, y, z);
    }
}