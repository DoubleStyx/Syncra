using System.Numerics;
using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Slider : ISystem
{
    public List<Type> Dependencies { get; } = new();
    public List<Type> Signature { get; }

    public void Update(Scene scene, Guid entity, ref Components.Transform transform, ref Components.Slider slider)
    {
        var origin = slider.Origin;
        var speed = slider.Speed;
        var amplitude = slider.Amplitude;
        var now = DateTime.Now.Ticks / 10000000.0f;
        var x = origin.X + amplitude.X * MathF.Sin(1000.0f / now * speed.X);
        var y = origin.Y + amplitude.Y * MathF.Sin(1000.0f / now * speed.Y);
        var z = origin.Z + amplitude.Z * MathF.Sin(1000.0f / now * speed.Z);
        transform.Position = new Vector3(x, y, z);
    }
}