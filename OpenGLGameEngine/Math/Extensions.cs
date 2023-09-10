using System.Numerics;
using NLog;

namespace OpenGLGameEngine.Math;

public static class Extensions
{
    public static Vector3 normalized(this Vector3 vector) => Vector3.Normalize(vector);

    public static void Normalize(this ref Vector3 vector)
    {
        vector = Vector3.Normalize(vector);
    }

    /// <summary>
    /// Rotate this quaternion <b>IN PLACE</b> by the given euler angles in radians.
    /// </summary>
    /// <param name="rotation">Quaternion to rotate</param>
    /// <param name="angles">X,Y,Z angles stored in a vector</param>
    /// <returns></returns>
    public static Quaternion Rotate(this ref Quaternion rotation, Vector3 angles)
    {
        var q = Quaternion.CreateFromYawPitchRoll(angles.X, angles.Y, angles.Z);
        return rotation *= q;
    }

    /// <summary>
    /// Rotate this quaternion <b>IN PLACE</b> by the given euler angles in degrees.
    /// </summary>
    /// <param name="rotation">Quaternion to rotate</param>
    /// <param name="angles">X,Y,Z angles stored in a vector</param>
    /// <returns></returns>
    public static Quaternion RotateEuler(this ref Quaternion rotation, Vector3 angles)
    {
        return rotation.Rotate(new Vector3(angles.X.ToRadians(), angles.Y.ToRadians(), angles.Z.ToRadians()));
    }


    /// <summary>
    /// Converts a quaternion to euler angles in radians.
    /// 
    /// Modified from https://stackoverflow.com/a/70462919
    /// </summary>
    /// <param name="q">Quaternion to convert</param>
    /// <returns></returns>
    public static Vector3 ToRadians(this Quaternion q)
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

    /// <summary>
    /// Converts a quaternion to euler angles in degrees.
    /// </summary>
    /// <param name="q">Quaternion to convert</param>
    /// <returns></returns>
    public static Vector3 ToDegrees(this Quaternion q)
    {
        var radians = q.ToRadians();
        return new Vector3(radians.X.ToDegrees(), radians.Z.ToDegrees(), radians.Y.ToDegrees());
    }

    /// <summary>
    /// Returns a vector by the given rotation in Quaternion form. <b>DOES NOT CHANGE ORIGINAL VECTOR</b>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angles"></param>
    /// <returns></returns>
    public static Vector3 Transform(this Vector3 vector, Quaternion rotation)
    {
        return Vector3.Transform(vector, rotation);
    }

    public static float ToRadians(this float degrees)
    {
        return (MathF.PI / 180f) * degrees;
    }

    public static float ToDegrees(this float radians)
    {
        return (180f / MathF.PI) * radians;
    }
}