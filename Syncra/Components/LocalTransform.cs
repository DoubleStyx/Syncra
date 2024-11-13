using System.Numerics;

namespace Syncra.Components;

public struct LocalTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public LocalTransform()
    {
        position = Vector3.Zero;
        rotation = Quaternion.Identity;
        scale = Vector3.One;
    }

    public LocalTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}