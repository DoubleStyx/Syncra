using System.Numerics;
using Syncra.SyncraEngine;

namespace SyncraCorePackages;

public struct Slider : IComponent
{
    public Vector3 Speed;

    public Guid Guid
    {
        get
        {
            return new Guid(new byte[] { 6, 2, 3, 4 });
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