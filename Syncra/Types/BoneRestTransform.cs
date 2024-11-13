using System.Numerics;

namespace Syncra.Types;

// Better namespace for this?
public struct BoneRestTransform
{
    public int Index;
    public int Parent;
    public Vector3 RestPosition;
    public Quaternion RestRotation;
    public Vector3 RestScale;
}