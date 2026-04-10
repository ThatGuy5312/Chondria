namespace Chondria.Math;

// simple math helper
public struct Mathf
{
    public static int Clamp(int v, int min, int max)
    {
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }

    public static float Clamp(float v, float min, float max)
    {
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }

    public static float Lerp(float a, float b, float t) => a + (b - a) * t;

    public static float DegreesToRadians(float degrees) => degrees * (MathF.PI / 180f);

    public static float RadiansToDegrees(float radians) => radians * (180f / MathF.PI);

    public static float Sin(float radians) => MathF.Sin(radians);

    public static float Cos(float radians) => MathF.Cos(radians);

    public static float Tan(float radians) => MathF.Tan(radians);

    public static float Asin(float value) => MathF.Asin(value);

    public static float Acos(float value) => MathF.Acos(value);

    public static float Atan(float x) => MathF.Atan(x);
    public static float Atan2(float y, float x) => MathF.Atan2(y, x);

    public static float Sqrt(float value) => MathF.Sqrt(value);

    public static float Abs(float value) => MathF.Abs(value);

    public static float Pow(float x, float y) => MathF.Pow(x, y);

    public static float Exp(float power) => MathF.Exp(power);

    public static float Log(float value) => MathF.Log(value);

    public static float Log10(float value) => MathF.Log10(value);

    public static float Floor(float value) => MathF.Floor(value);

    public static float Ceiling(float value) => MathF.Ceiling(value);

    public static float Round(float value) => MathF.Round(value);

    public static float Sign(float value) => MathF.Sign(value);

    public static float Max(float a, float b) => MathF.Max(a, b);

    public static float Min(float a, float b) => MathF.Min(a, b);

    public static int Max(int a, int b) => (int)MathF.Max(a, b);

    public static int Min(int a, int b) => (int)MathF.Min(a, b);

}
