namespace Chondria.Math;


// typical 2d vector and holds a x and y value. This is used for things like texture coordinates, 2d positions, etc.
public struct Vector2
{
    public float X;
    public float Y;

    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);
    public static Vector2 UnitX => new(1, 0);
    public static Vector2 UnitY => new(0, 1);

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vector2(Vector3 xy)
    {
        X = xy.X;
        Y = xy.Y;
    }

    #region Implicit conversions and math operators

    public static implicit operator OpenTK.Mathematics.Vector2(Vector2 v) => new(v.X, v.Y);
    public static implicit operator Vector2(OpenTK.Mathematics.Vector2 v) => new(v.X, v.Y);
    public static implicit operator Vector2(System.Numerics.Vector2 v) => new(v.X, v.Y);
    public static implicit operator System.Numerics.Vector2(Vector2 v) => new(v.X, v.Y);

    public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);

    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2 operator -(Vector2 a, Vector2 b)  => new(a.X - b.X, a.Y - b.Y);

    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);

    public static Vector2 operator *(Vector2 a, float b) => new(a.X * b, a.Y * b);
    public static Vector2 operator /(Vector2 a, float b) => new(a.X / b, a.Y / b);

    public static Vector2 operator +(Vector2 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2 operator -(Vector2 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y);

    #endregion
}
