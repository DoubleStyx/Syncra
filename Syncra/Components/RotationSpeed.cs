using System.Numerics;

namespace Syncra.Components;

public struct RotationSpeed
{
    public Vector3 value;

    public RotationSpeed()
    {
        value = Vector3.Zero;
    }
    
    public RotationSpeed(Vector3 value)
    {
        this.value = value;
    }
}