namespace Chondria.Math;

// same as Vector4 but intergers
public struct Vector4i
{
    public int X;
    public int Y;
    public int Z;
    public int W;
    public Vector4i(int x, int y, int z, int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4i(Vector3i xyz, int w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    public Vector4i(Vector2i xy, int z, int w)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    #region Implicit conversions and math operators

    // Thank you copliot for recommending this. I didn't know this was a thing, and I absolutely love it. I'm adding this to all my vector structs now.
    public static implicit operator OpenTK.Mathematics.Vector4i(Vector4i v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector4i(OpenTK.Mathematics.Vector4i v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector4i(System.Numerics.Vector4 v) => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);
    public static implicit operator System.Numerics.Vector4(Vector4i v) => new(v.X, v.Y, v.Z, v.W);

    public static implicit operator Vector4(Vector4i v) => new(v.X, v.Y, v.Z, v.W);

    // math operators
    public static Vector4i operator +(Vector4i a, Vector4i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Vector4i operator -(Vector4i a, Vector4i b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Vector4i operator *(Vector4i a, Vector4i b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    public static Vector4i operator /(Vector4i a, Vector4i b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    public static Vector4i operator *(Vector4i a, int b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);
    public static Vector4i operator /(Vector4i a, int b) => new(a.X / b, a.Y / b, a.Z / b, a.W / b);

    public static Vector4i operator +(Vector4i a, Vector3i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W);
    public static Vector4i operator -(Vector4i a, Vector3i b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W);
    public static Vector4i operator *(Vector4i a, Vector3i b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W);
    public static Vector4i operator /(Vector4i a, Vector3i b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W);

    public static Vector4i operator +(Vector4i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y, a.Z, a.W);
    public static Vector4i operator -(Vector4i a, Vector2i b) => new(a.X - b.X, a.Y - b.Y, a.Z, a.W);
    public static Vector4i operator *(Vector4i a, Vector2i b) => new(a.X * b.X, a.Y * b.Y, a.Z, a.W);
    public static Vector4i operator /(Vector4i a, Vector2i b) => new(a.X / b.X, a.Y / b.Y, a.Z, a.W);

    #endregion
}
