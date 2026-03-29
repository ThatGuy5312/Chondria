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

    public static Vector3 RotateX(Vector3 v, float angle)
    {
        float cos = (float)System.Math.Cos(angle);
        float sin = (float)System.Math.Sin(angle);

        return new Vector3(
            v.X,
            v.Y * cos - v.Z * sin,
            v.X * sin + v.Z * cos
        );
    }

    public static Vector3 RotateY(Vector3 v, float angle)
    {
        float cos = (float)System.Math.Cos(angle);
        float sin = (float)System.Math.Sin(angle);

        return new Vector3(
            v.X * cos - v.Z * sin,
            v.Y,
            v.X * sin + v.Z * cos
        );
    }

    public static Vector3 RotateZ(Vector3 v, float angle)
    {
        float cos = (float)System.Math.Cos(angle);
        float sin = (float)System.Math.Sin(angle);

        return new Vector3(
            v.X * cos - v.Z * sin,
            v.Y * cos - v.Z * sin,
            v.Y
        );
    }
}
