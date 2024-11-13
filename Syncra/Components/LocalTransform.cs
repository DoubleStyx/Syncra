using System.Numerics;

namespace Syncra.Components;

public struct LocalTransform
{
    public Matrix4x4 value;

    public LocalTransform()
    {
        this.value = Matrix4x4.Identity;
    }

    public LocalTransform(Matrix4x4 value)
    {
        this.value = value;
    }
}