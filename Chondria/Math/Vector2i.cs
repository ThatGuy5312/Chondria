using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chondria.Math;

public struct Vector2i
{
    public int X;
    public int Y;

    public static Vector2i Zero => new(0, 0);
    public static Vector2i One => new(1, 1);
    public static Vector2i UnitX => new(1, 0);
    public static Vector2i UnitY => new(0, 1);

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2i(Vector3i xy)
    {
        X = xy.X;
        Y = xy.Y;
    }

    #region Implicit conversions and math operators

    public static implicit operator OpenTK.Mathematics.Vector2i(Vector2i v) => new(v.X, v.Y);
    public static implicit operator Vector2i(OpenTK.Mathematics.Vector2i v) => new(v.X, v.Y);
    public static implicit operator Vector2i(System.Numerics.Vector2 v) => new((int)v.X, (int)v.Y);
    public static implicit operator System.Numerics.Vector2(Vector2i v) => new(v.X, v.Y);

    public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);

    public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2i operator -(Vector2i a, Vector2i b) => new(a.X - b.X, a.Y - b.Y);

    public static Vector2i operator *(Vector2i a, Vector2i b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2i operator /(Vector2i a, Vector2i b) => new(a.X / b.X, a.Y / b.Y);

    public static Vector2i operator *(Vector2i a, int b) => new(a.X * b, a.Y * b);
    public static Vector2i operator /(Vector2i a, int b) => new(a.X / b, a.Y / b);

    public static Vector2i operator +(Vector2i a, Vector3i b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2i operator -(Vector2i a, Vector3i b) => new(a.X - b.X, a.Y - b.Y);

    #endregion
}
