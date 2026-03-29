namespace Chondria.Math;


// the typical but more powerful 3d vector and holds a x, y, and z value. This is used for things like positions, normals, etc.
public struct Vector3
{
    public float X;
    public float Y;
    public float Z;

    public static Vector3 Zero => new(0, 0, 0);
    public static Vector3 One => new(1, 1, 1);
    public static Vector3 UnitX => new(1, 0, 0);
    public static Vector3 UnitY => new(0, 1, 0);
    public static Vector3 UnitZ => new(0, 0, 1);

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(Vector2 xy, float z)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
    }

    #region Implicit conversions and math operators

    public static implicit operator OpenTK.Mathematics.Vector3(Vector3 v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector3(OpenTK.Mathematics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector3(System.Numerics.Vector3 v) => new(v.X, v.Y, v.Z);
    public static implicit operator System.Numerics.Vector3(Vector3 v) => new(v.X, v.Y, v.Z);


    public static Vector3 operator -(Vector3 a) => new(-a.X, -a.Y, -a.Z);


    public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3 operator *(Vector3 a, Vector3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vector3 operator /(Vector3 a, Vector3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    public static Vector3 operator *(Vector3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vector3 operator /(Vector3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);

    public static Vector3 operator +(Vector3 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y, a.Z);

    public static Vector3 operator -(Vector3 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y, a.Z);

    #endregion
}
