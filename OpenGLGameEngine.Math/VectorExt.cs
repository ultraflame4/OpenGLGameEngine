using System.Numerics;

namespace OpenGLGameEngine.Math;

/**
 * Collection of extensions and helper functions for System.Numerics.Vector types
 */
public static class VectorExt
{
    public static Vector2 toVec2(this (float x, float y) xy) { return new Vector2(xy.x, xy.y); }
    public static Vector2 fromXY(int x, int y) { return new Vector2(x, y); }
    public static Vector3 normalized(this Vector3 vector) => Vector3.Normalize(vector);
    public static void Normalize(this ref Vector3 vector) { vector = Vector3.Normalize(vector); }

    /// <summary>
    /// Convert angles in degrees stored in a vector2 to radians by applying ToRad to each component of the vector
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector2 ToRad(this Vector2 degrees)
    {
        return new Vector2(degrees.X.ToRad(), degrees.Y.ToRad());
    }
    /// <summary>
    /// Convert angles in degrees stored in a vector3 to radians by applying ToRad to each component of the vector
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector3 ToRad(this Vector3 degrees)
    {
        return new Vector3(degrees.X.ToRad(), degrees.Y.ToRad(), degrees.Z.ToRad());
    }
    /// <summary>
    /// Convert angles in radians stored in a vector2 to degrees by applying ToDeg to each component of the vector
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static Vector2 ToDeg(this Vector2 radians)
    {
        return new Vector2(radians.X.ToDeg(), radians.Y.ToDeg());
    }
    /// <summary>
    /// Convert angles in radians stored in a vector3 to degrees by applying ToDeg to each component of the vector
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static Vector3 ToDeg(this Vector3 radians)
    {
        return new Vector3(radians.X.ToDeg(), radians.Y.ToDeg(), radians.Z.ToDeg());
    }
}