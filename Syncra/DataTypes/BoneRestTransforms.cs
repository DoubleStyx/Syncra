using System.Numerics;
using Syncra.Networking;

namespace Syncra.DataTypes;

public struct BoneRestTransforms
{
    public int Index;
    public int Parent;
    public Vector3 RestPosition;
    public Quaternion RestRotation;
    public Vector3 RestScale;
}
