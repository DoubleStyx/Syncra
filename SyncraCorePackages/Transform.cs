using System.Numerics;
using Syncra.SyncraEngine;

namespace SyncraCorePackages;

public struct Transform : IComponent
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Transform()
    {
        Position = Vector3.Zero;
        Rotation = Vector3.Zero;
        Scale = Vector3.One;
    }

    public Guid Guid
    {
        get
        {
            return new Guid(new byte[] { 1, 2, 3, 4 });
        }
    }

    public IReadOnlyList<Guid> Dependencies
    {
        get
        {
            return new List<Guid>();
        }
    }
}