using System.Numerics;

namespace Syncra.Components;

/// <summary>
/// A component defining the properties of the spinner.
/// </summary>
public struct Spinner : IComponent
{
    /// <summary>
    /// The rotation speed in euler angles in radians per second.
    /// </summary>
    public Vector3 Speed;

    /// <summary>
    /// Creates a new spinner component with no speed by default.
    /// </summary>
    public Spinner()
    {
        Speed = Vector3.Zero;
    }

    /// <summary>
    /// Creates a new spinner component according to the input speed.
    /// </summary>
    /// <param name="speed"></param>
    public Spinner(Vector3 speed)
    {
        Speed = speed;
    }
}