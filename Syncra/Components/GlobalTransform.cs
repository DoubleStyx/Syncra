using System.Numerics;

namespace Syncra.Components;

public struct GlobalTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public bool dirty;

    public GlobalTransform()
    {
        position = Vector3.Zero;
        rotation = Quaternion.Identity;
        scale = Vector3.One;
        dirty = true;
    }

    public GlobalTransform(Vector3 position, Quaternion rotation, Vector3 scale, bool dirty)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.dirty = dirty;
    }
}