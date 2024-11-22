using System.Numerics;
using SyncraEngine;

namespace SyncraOfficialPackages.Systems;

public class Slider : ISystem
{
    public void Update(Scene scene, Guid entity, ref Components.Transform transform, ref Components.Slider slider)
    {
        var origin = slider.Origin;
        var speed = slider.Speed;
        var amplitude = slider.Amplitude;
        var now = DateTime.Now.Ticks / 10000000;
        var X = origin.X + amplitude.X * MathF.Sin(1000 / now * speed.X);
        var Y = origin.Y + amplitude.Y * MathF.Sin(1000 / now * speed.Y);
        var Z = origin.Z + amplitude.Z * MathF.Sin(1000 / now * speed.Z);
        transform.Position = new Vector3(X, Y, Z);
    }
}