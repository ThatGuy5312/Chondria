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

    public static implicit operator Vector3(Vector3i v) => new(v.X, v.Y, v.Z);


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

    #region instance functions

    public float Length() => Mathf.Sqrt(X * X + Y * Y + Z * Z);

    public float Dot(Vector3 other) => X * other.X + Y * other.Y + Z * other.Z;

    public Vector3 Cross(Vector3 other) => new(
        Y * other.Z - Z * other.Y,
        Z * other.X - X * other.Z,
        X * other.Y - Y * other.X
    );

    public Vector3 Reflect(Vector3 normal) => this - normal * (2 * Dot(normal));

    public Vector3 Normalized()
    {
        float length = Length();
        if (length == 0) return Zero;
        return this / length;
    }

    #endregion

    #region static functions

    public static float Distance(Vector3 a, Vector3 b) => (a - b).Length();

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;

    public static Vector3 Min(Vector3 a, Vector3 b) => new(
        Mathf.Min(a.X, b.X),
        Mathf.Min(a.Y, b.Y),
        Mathf.Min(a.Z, b.Z)
    );

    public static Vector3 Max(Vector3 a, Vector3 b) => new(
        Mathf.Max(a.X, b.X),
        Mathf.Max(a.Y, b.Y),
        Mathf.Max(a.Z, b.Z)
    );

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) => new(
        Mathf.Max(min.X, Mathf.Min(max.X, value.X)),
        Mathf.Max(min.Y, Mathf.Min(max.Y, value.Y)),
        Mathf.Max(min.Z, Mathf.Min(max.Z, value.Z))
    );

    public static Vector3 Cross(Vector3 a, Vector3 b) => a.Cross(b);

    public static float Dot(Vector3 a, Vector3 b) => a.Dot(b);

    public static Vector3 Reflect(Vector3 vector, Vector3 normal) => vector.Reflect(normal);

    public static Vector3 Normalize(Vector3 v) => v.Normalized();

    #endregion
}
