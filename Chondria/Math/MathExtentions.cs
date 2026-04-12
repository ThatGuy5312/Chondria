namespace Chondria.Math;


// simple math extentions. just for conversions right now.
public static class MathExtentions
{
    public static System.Numerics.Vector2 Numerics(this Vector2 v) => new(v.X, v.Y);
    public static OpenTK.Mathematics.Vector2 TK(this Vector2 v) => new(v.X, v.Y);
    public static OpenTK.Mathematics.Vector2 TK(this System.Numerics.Vector2 v) => new(v.X, v.Y);
    public static System.Numerics.Vector2 Numerics(this OpenTK.Mathematics.Vector2 v) => new(v.X, v.Y);
    public static Vector2 ChonVec(this System.Numerics.Vector2 v) => new(v.X, v.Y);
    public static Vector2 ChonVec(this OpenTK.Mathematics.Vector2 v) => new(v.X, v.Y);

    public static System.Numerics.Vector3 Numerics(this Vector3 v) => new(v.X, v.Y, v.Z);
    public static OpenTK.Mathematics.Vector3 TK(this Vector3 v) => new(v.X, v.Y, v.Z);
    public static Vector3 ChonVec(this System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static Vector3 ChonVec(this OpenTK.Mathematics.Vector3 v) => new(v.X, v.Y, v.Z);
}
