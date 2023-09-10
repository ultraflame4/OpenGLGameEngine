using System.Numerics;

namespace OpenGLGameEngine.Math;

public static class Extensions
{
    public static Vector3 normalized(this Vector3 vector)  => Vector3.Normalize(vector);
    
    public static void Normalize(this ref Vector3 vector)
    {
        vector = Vector3.Normalize(vector);
    }
    
    public static Quaternion Add(this ref Quaternion rotation, Quaternion amt)
    {
        return rotation=Quaternion.Add(rotation, amt);
    }

    public static Vector3 ToDirection(this Quaternion rotation, Vector3? up = null)
    {
        return Vector3.Transform(up??Vector3.UnitY, rotation);
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