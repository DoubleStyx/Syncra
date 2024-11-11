using System.Numerics;

namespace Syncra.Components;

public struct SpinnerNode : IComponent
{
    public Node Node { get; set; }
    public Vector3 RotationSpeed;
}