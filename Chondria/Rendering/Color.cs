using Chondria.Math;

namespace Chondria.Rendering;

// Color struct that hold RGBA values in 8 and 32 bit values. Will improve later.
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

    public static implicit operator OpenTK.Mathematics.Vector3(Color c) => new(c.Rf, c.Gf, c.Bf);
    public static implicit operator OpenTK.Mathematics.Vector4(Color c) => new(c.Rf, c.Gf, c.Bf, c.Af);

    public static implicit operator System.Numerics.Vector3(Color c) => new(c.Rf, c.Gf, c.Bf);
    public static implicit operator System.Numerics.Vector4(Color c) => new(c.Rf, c.Gf, c.Bf, c.Af);

    public static implicit operator Color(System.Numerics.Vector3 c) => new(c.X, c.Y, c.Z);
    public static implicit operator Color(System.Numerics.Vector4 c) => new(c.X, c.Y, c.Z, c.W);

    public static implicit operator System.Drawing.Color(Color c) => System.Drawing.Color.FromArgb(c.R, c.G, c.B);
    public static implicit operator Color(System.Drawing.Color c) => new(c.R, c.G, c.B, c.A);

    public static Color White => new(255, 255, 255);
    public static Color Black => new(0, 0, 0);

    public static Color Red => new(255, 0, 0);
    public static Color Green => new(0, 255, 0);
    public static Color Blue => new(0, 0, 255);

    public static Color Yellow => new(255, 255, 0);
    public static Color Cyan => new(0, 255, 255);
    public static Color Magenta => new(255, 0, 255);
    public static Color Orange => new(255, 165, 0);
    public static Color Purple => new(128, 0, 128);
    public static Color Pink => new(255, 192, 203);
    public static Color Brown => new(165, 42, 42);
    public static Color Gray => new(128, 128, 128);
    public static Color LightGray => new(211, 211, 211);
    public static Color DarkGray => new(64, 64, 64);
    public static Color Lime => new(0, 255, 0); // isn't this just green?
    public static Color Olive => new(128, 128, 0);
    public static Color Navy => new(0, 0, 128);
    public static Color Teal => new(0, 128, 128);
    public static Color Maroon => new(128, 0, 0);
    public static Color Gold => new(255, 215, 0);
    public static Color Silver => new(192, 192, 192);
    public static Color Coral => new(255, 127, 80);
    public static Color Indigo => new(75, 0, 130);
    public static Color Violet => new(238, 130, 238);
    public static Color Turquoise => new(64, 224, 208);
    public static Color Wheat => new(245, 222, 179);
    public static Color Salmon => new(250, 128, 114);
    public static Color Sandy => new(244, 164, 96);
    public static Color ForestGreen => new(34, 139, 34);

    public static Color Lerp(Color a, Color b, float t) => new(
    (byte)(a.R + (b.R - a.R) * t),
    (byte)(a.G + (b.G - a.G) * t),
    (byte)(a.B + (b.B - a.B) * t),
    (byte)(a.A + (b.A - a.A) * t));
}
