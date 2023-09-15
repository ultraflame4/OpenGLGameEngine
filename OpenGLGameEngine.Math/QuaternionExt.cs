using System.Numerics;

namespace OpenGLGameEngine.Math;

public static class QuaternionExt
{
    /// <summary>
    /// Rotates this quaternion <b>IN PLACE</b> by another quaternion.
    /// In other words, this = this * b.
    /// Or in an
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Quaternion Rotate(this ref Quaternion a, Quaternion b)
    {
        return a *= b;
    }

    /// <summary>
    /// Rotates this quaternion <b>IN PLACE</b> by euler angles in radians.
    /// In other words, this = this * b.
    /// Or in an
    /// </summary>
    /// <param name="a">Current quaternion to be rotated</param>
    /// <param name="angles">Angles in radians to rotate this quaternion by</param>
    /// <returns></returns>
    public static Quaternion RotateEuler(this ref Quaternion a, Vector3 angles)
    {
        var q = Quaternion.CreateFromYawPitchRoll(angles.X, angles.Y, angles.Z);
        return a *= q;
    }
    /// <summary>
    /// Converts a quaternion to euler angles in radians.
    /// 
    /// Modified from https://stackoverflow.com/a/70462919
    /// </summary>
    /// <param name="q">Quaternion to convert</param>
    /// <returns></returns>
    public static Vector3 ToEuler(this Quaternion q)
    {
        Vector3 angles = new();

        // roll / x
        double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
        double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        angles.X = (float)System.Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch / y
        double sinp = 2 * (q.W * q.Y - q.Z * q.X);
        if (System.Math.Abs(sinp) >= 1)
        {
            angles.Y = (float)System.Math.CopySign(System.Math.PI / 2, sinp);
        }
        else
        {
            angles.Y = (float)System.Math.Asin(sinp);
        }

        // yaw / z
        double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
        double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        angles.Z = (float)System.Math.Atan2(siny_cosp, cosy_cosp);

        return angles;
    }
}