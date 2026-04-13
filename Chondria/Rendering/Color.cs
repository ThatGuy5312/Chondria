using Chondria.Math;

namespace Chondria.Rendering;

public struct Color
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public float Rf;
    public float Gf;
    public float Bf;
    public float Af;

    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
        Set32Values();
    }

    public Color(float r, float g, float b, float a = 1f)
    {
        R = (byte)(r * 255);
        G = (byte)(g * 255);
        B = (byte)(b * 255);
        A = (byte)(a * 255);
        Set32Values();
    }

    void Set32Values()
    {
        Rf = R / 255f;
        Gf = G / 255f;
        Bf = B / 255f;
        Af = A / 255f;
    }

    public static implicit operator Color(Vector3 v) => new(v.X, v.Y, v.Z);
    public static implicit operator Color(Vector4 v) => new(v.X, v.Y, v.Z, v.W);
    public static implicit operator Vector3(Color c) => new(c.Rf, c.Gf, c.Bf);
    public static implicit operator Vector4(Color c) => new(c.Rf, c.Gf, c.Bf, c.Af);

    public static Color Lerp(Color a, Color b, float t) => new(
    (byte)(a.R + (b.R - a.R) * t),
    (byte)(a.G + (b.G - a.G) * t),
    (byte)(a.B + (b.B - a.B) * t),
    (byte)(a.A + (b.A - a.A) * t));
}
