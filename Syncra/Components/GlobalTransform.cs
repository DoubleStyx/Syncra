using System.Numerics;

namespace Syncra.Components;

public struct GlobalTransform
{
    public Matrix4x4 value;
    public bool dirty;

    public GlobalTransform()
    {
        value = Matrix4x4.Identity;   
        dirty = true;
    }

    public GlobalTransform(Matrix4x4 value, bool dirty)
    {
        this.value = value;
        this.dirty = dirty;
    }
}