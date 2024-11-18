using System.Numerics;

namespace Syncra.Components;

public struct Spinner : IComponent
{
    public Vector3 Speed;

    public Spinner()
    {
        Speed = Vector3.Zero;
    }

    public Spinner(Vector3 speed)
    {
        Speed = speed;
    }
}