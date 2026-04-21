using Chondria.Rendering;

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
    public static OpenTK.Mathematics.Vector3 TK(this System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static System.Numerics.Vector3 Numerics(this OpenTK.Mathematics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static Vector3 ChonVec(this System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static Vector3 ChonVec(this OpenTK.Mathematics.Vector3 v) => new(v.X, v.Y, v.Z);

    public static System.Numerics.Vector3 Numerics(this Color v) => new(v.Rf, v.Gf, v.Bf);
    public static OpenTK.Mathematics.Vector3 TK(this Color v) => new(v.Rf, v.Gf, v.Bf);
    public static System.Numerics.Vector4 Numerics4(this Color v) => new(v.Rf, v.Gf, v.Bf, v.Af);
    public static OpenTK.Mathematics.Vector4 TK4(this Color v) => new(v.Rf, v.Gf, v.Bf, v.Af);
    public static Color Numerics(this System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static Color TK(this OpenTK.Mathematics.Vector3 v) => new(v.X, v.Y, v.Z);

    public static OpenTK.Mathematics.Matrix4 TK(this Matrix4 m) => new(m.Row0, m.Row1, m.Row2, m.Row3);
    public static Matrix4 ChonMat(this OpenTK.Mathematics.Matrix4 m) => new(m.Row0, m.Row1, m.Row2, m.Row3);
}
