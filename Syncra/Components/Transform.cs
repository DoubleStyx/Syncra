using System.Numerics;

namespace Syncra.Components;

public struct Transform : IComponent
{
    public Matrix4x4 Matrix;

    public Transform()
    {
        Matrix = Matrix4x4.Identity;
    }

    public Transform(Matrix4x4 matrix)
    {
        Matrix = matrix;
    }

    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Matrix = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix4x4.CreateTranslation(position);
    }
}