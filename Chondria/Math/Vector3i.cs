namespace Chondria.Math;

// same as Vector3 but intergers
public struct Vector3i
{
    public int X;
    public int Y;
    public int Z;

    public static Vector3i Zero => new(0, 0, 0);
    public static Vector3i One => new(1, 1, 1);
    public static Vector3i UnitX => new(1, 0, 0);
    public static Vector3i UnitY => new(0, 1, 0);
    public static Vector3i UnitZ => new(0, 0, 1);

    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3i(Vector2i xy, int z)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
    }

    #region Implicit conversions and math operators

    public static implicit operator OpenTK.Mathematics.Vector3(Vector3i v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector3i(OpenTK.Mathematics.Vector3i v) => new(v.X, v.Y, v.Z);
    public static implicit operator Vector3i(System.Numerics.Vector3 v) => new((int)v.X, (int)v.Y, (int)v.Z);
    public static implicit operator System.Numerics.Vector3(Vector3i v) => new(v.X, v.Y, v.Z);

    public static implicit operator Vector3(Vector3i v) => new(v.X, v.Y, v.Z);


    public static Vector3i operator -(Vector3i a) => new(-a.X, -a.Y, -a.Z);


    public static Vector3i operator +(Vector3i a, Vector3i b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3i operator -(Vector3i a, Vector3i b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3i operator *(Vector3i a, Vector3i b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vector3i operator /(Vector3i a, Vector3i b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

    public static Vector3i operator *(Vector3i a, int b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vector3i operator /(Vector3i a, int b) => new(a.X / b, a.Y / b, a.Z / b);

    public static Vector3i operator +(Vector3i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y, a.Z);

    public static Vector3i operator -(Vector3i a, Vector2i b) => new(a.X - b.X, a.Y - b.Y, a.Z);

    #endregion

        #region instance functions

    public int Length() => (int)Mathf.Sqrt(X * X + Y * Y + Z * Z);

    public int Dot(Vector3i other) => X * other.X + Y * other.Y + Z * other.Z;

    public Vector3i Cross(Vector3i other) => new(
        Y * other.Z - Z * other.Y,
        Z * other.X - X * other.Z,
        X * other.Y - Y * other.X
    );

    public Vector3i Reflect(Vector3i normal) => this - normal * (2 * Dot(normal));

    public Vector3i Normalized()
    {
        int length = Length();
        if (length == 0) return Zero;
        return this / length;
    }

    #endregion

    #region static functions

    public static float Distance(Vector3i a, Vector3i b) => (a - b).Length();

    public static Vector3i Lerp(Vector3i a, Vector3i b, int t) => a + (b - a) * t;

    public static Vector3i Min(Vector3i a, Vector3i b) => new(
        Mathf.Min(a.X, b.X),
        Mathf.Min(a.Y, b.Y),
        Mathf.Min(a.Z, b.Z)
    );

    public static Vector3i Max(Vector3i a, Vector3i b) => new(
        Mathf.Max(a.X, b.X),
        Mathf.Max(a.Y, b.Y),
        Mathf.Max(a.Z, b.Z)
    );

    public static Vector3i Clamp(Vector3i value, Vector3i min, Vector3i max) => new(
        Mathf.Max(min.X, Mathf.Min(max.X, value.X)),
        Mathf.Max(min.Y, Mathf.Min(max.Y, value.Y)),
        Mathf.Max(min.Z, Mathf.Min(max.Z, value.Z))
    );

    public static Vector3i Cross(Vector3i a, Vector3i b) => a.Cross(b);

    public static float Dot(Vector3i a, Vector3i b) => a.Dot(b);

    public static Vector3i Reflect(Vector3i vector, Vector3i normal) => vector.Reflect(normal);

    public static Vector3i Normalize(Vector3i v) => v.Normalized();

    #endregion
}
