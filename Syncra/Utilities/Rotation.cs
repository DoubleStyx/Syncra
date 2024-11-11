using System.Numerics;

namespace Syncra.Math;

public static class Rotation
{
    public static Quaternion ToQuaternion(this Vector3 euler) // pitch yaw roll
    {
        return Quaternion.CreateFromYawPitchRoll(euler.Y, euler.X, euler.Z); // yaw pitch roll
    }

    private static float ComputeXAngle(this Quaternion q)
    {
        float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
        float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        return MathF.Atan2(sinr_cosp, cosr_cosp);
    }

    private static float ComputeYAngle(this Quaternion q)
    {
        float sinp = 2 * (q.W * q.Y - q.Z * q.X);
        if (MathF.Abs(sinp) >= 1)
            return MathF.PI / 2 * MathF.Sign(sinp); // Use 90 degrees if out of range
        else
            return MathF.Asin(sinp);
    }

    private static float ComputeZAngle(this Quaternion q)
    {
        float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
        float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        return MathF.Atan2(siny_cosp, cosy_cosp);
    }

    public static Vector3 ToEuler(this Quaternion q)
    {
        return new Vector3(ComputeXAngle(q), ComputeYAngle(q), ComputeZAngle(q));
    }
}