using System.Numerics;
using Syncra.Networking;

namespace Syncra.Components;

public struct TransformComponent : INetworkedComponent
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public void WriteAll(Stream stream)
    {
        stream.Write(Position);
        stream.Write(Rotation);
        stream.Write(Scale);
    }
    public void ReadAll(Stream stream)
    {
        stream.Read(out Position);
        stream.Read(out Rotation);
        stream.Read(out Scale);
    }
}