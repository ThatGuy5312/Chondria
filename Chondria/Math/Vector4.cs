namespace Chondria.Math;

// the typical but more powerful 4d vector and holds a x, y, z, and w value. This is used for things like colors, quaternions, etc.
public struct Vector4
{
    public float X;
    public float Y;
    public float Z;
    public float W;
    public Vector4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4(Vector3 xyz, float w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    public Vector4(Vector2 xy, float z, float w)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    #region Implicit conversions and math operators

    // Thank you copliot for recommending this. I didn't know this was a thing, and I absolutely love it. I'm adding this to all my vector structs now.
    public static implicit operator OpenTK.Mathematics.Vector4(Vector4 v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector4(OpenTK.Mathematics.Vector4 v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector4(System.Numerics.Vector4 v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator System.Numerics.Vector4(Vector4 v) => new(v.X, v.Y, v.Z, v.W);

    public static implicit operator Vector4(Vector4i v) => new(v.X, v.Y, v.Z, v.W);

    // math operators
    public static Vector4 operator +(Vector4 a, Vector4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Vector4 operator -(Vector4 a, Vector4 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Vector4 operator *(Vector4 a, Vector4 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    public static Vector4 operator /(Vector4 a, Vector4 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    public static Vector4 operator *(Vector4 a, float b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);
    public static Vector4 operator /(Vector4 a, float b) => new(a.X / b, a.Y / b, a.Z / b, a.W / b);

    public static Vector4 operator +(Vector4 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W);
    public static Vector4 operator -(Vector4 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W);
    public static Vector4 operator *(Vector4 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W);
    public static Vector4 operator /(Vector4 a, Vector3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W);

    public static Vector4 operator +(Vector4 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y, a.Z, a.W);
    public static Vector4 operator -(Vector4 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y, a.Z, a.W);
    public static Vector4 operator *(Vector4 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y, a.Z, a.W);
    public static Vector4 operator /(Vector4 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y, a.Z, a.W);

    #endregion
}
