using System.Numerics;

namespace Syncra.Components;

/// <summary>
/// Represents a 4x4 transform matrix for a 3D scene.
/// </summary>
public struct Transform : IComponent
{
    /// <summary>
    /// The 4x4 transform matrix.
    /// </summary>
    public Matrix4x4 Matrix;

    /// <summary>
    /// Creates a new transform component using the identity matrix.
    /// </summary>
    public Transform()
    {
        Matrix = Matrix4x4.Identity;
    }

    /// <summary>
    /// Creates a new transform component using an input matrix.
    /// </summary>
    /// <param name="matrix"></param>
    public Transform(Matrix4x4 matrix)
    {
        Matrix = matrix;
    }

    /// <summary>
    /// Creates a new transform component using parameters to construct the matrix.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="scale"></param>
    public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        Matrix = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                 Matrix4x4.CreateTranslation(position);
    }
}