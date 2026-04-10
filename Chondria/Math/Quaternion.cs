namespace Chondria.Math;

public struct Quaternion
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static Quaternion Identity => new(0, 0, 0, 1);

    public Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    #region Implicit conversions and math operators

    public static implicit operator OpenTK.Mathematics.Quaternion(Quaternion q) => new(q.X, q.Y, q.Z, q.W);
    public static implicit operator Quaternion(OpenTK.Mathematics.Quaternion q) => new(q.X, q.Y, q.Z, q.W);
    public static implicit operator Quaternion(System.Numerics.Quaternion q) => new(q.X, q.Y, q.Z, q.W);
    public static implicit operator System.Numerics.Quaternion(Quaternion q) => new(q.X, q.Y, q.Z, q.W);

    public static Quaternion operator +(Quaternion a, Quaternion b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);

    public static Quaternion operator -(Quaternion a, Quaternion b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

    public static Quaternion operator -(Quaternion q) => new(-q.X, -q.Y, -q.Z, -q.W);

    public static Quaternion operator *(Quaternion q, float s) => new(q.X * s, q.Y * s, q.Z * s, q.W * s);
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
        );
    }

    public static Quaternion operator /(Quaternion q, float s) => new(q.X / s, q.Y / s, q.Z / s, q.W / s);
    public static Quaternion operator /(Quaternion a, Quaternion b)
    {
        return a * Conjugate(b) / (b.X * b.X + b.Y * b.Y + b.Z * b.Z + b.W * b.W);
    }

    public static Vector3 operator *(Quaternion q, Vector3 v)
    {
        // Convert the vector to a quaternion with w = 0
        Quaternion vQuat = new(v.X, v.Y, v.Z, 0);
        // Rotate the vector by the quaternion
        Quaternion resultQuat = q * vQuat * Conjugate(q);
        return new Vector3(resultQuat.X, resultQuat.Y, resultQuat.Z);
    }

    public static Quaternion operator *(Quaternion q, Vector4 vector)
    {
        // Convert the vector to a quaternion with w = 0
        Quaternion vQuat = new(vector.X, vector.Y, vector.Z, 0);
        // Rotate the vector by the quaternion
        Quaternion resultQuat = q * vQuat * Conjugate(q);
        return new Quaternion(resultQuat.X, resultQuat.Y, resultQuat.Z, vector.W);
    }

    #endregion

    #region instance functions

    public Quaternion Normalized()
    {
        float length = Mathf.Sqrt(X * X + Y * Y + Z * Z + W * W);
        if (length > 0)
        {
            float invLength = 1.0f / length;
            return new Quaternion(X * invLength, Y * invLength, Z * invLength, W * invLength);
        }
        return Identity;
    }

    public Quaternion Conjugate()
    {
        return new Quaternion(-X, -Y, -Z, W);
    }

    public Quaternion Inverse()
    {
        float lengthSq = X * X + Y * Y + Z * Z + W * W;
        if (lengthSq > 0)
        {
            float invLengthSq = 1.0f / lengthSq;
            return new Quaternion(-X * invLengthSq, -Y * invLengthSq, -Z * invLengthSq, W * invLengthSq);
        }
        return Identity;
    }

    public Vector3 ToEulerAngles()
    {
        float pitch = Mathf.Atan2(2 * (W * X + Y * Z), 1 - 2 * (X * X + Y * Y));
        float yaw = Mathf.Asin(2 * (W * Y - Z * X));
        float roll = Mathf.Atan2(2 * (W * Z + X * Y), 1 - 2 * (Y * Y + Z * Z));
        return new Vector3(Mathf.RadiansToDegrees(pitch), Mathf.RadiansToDegrees(yaw), Mathf.RadiansToDegrees(roll));
    }

    public void ToAxisAngle(out Vector3 axis, out float angle)
    {
        if (MathF.Abs(W) > 1)
            Normalized();
        angle = 2 * Mathf.Acos(W);
        float s = Mathf.Sqrt(1 - W * W);
        if (s < 0.001f)
        {
            axis = new Vector3(X, Y, Z);
        }
        else
        {
            axis = new Vector3(X / s, Y / s, Z / s);
        }
    }

    #endregion

    #region static functions

    public static Quaternion FromEulerAngles(Vector3 euler)
    {
        float c1 = Mathf.Cos(Mathf.DegreesToRadians(euler.Y) / 2);
        float s1 = Mathf.Sin(Mathf.DegreesToRadians(euler.Y) / 2);
        float c2 = Mathf.Cos(Mathf.DegreesToRadians(euler.X) / 2);
        float s2 = Mathf.Sin(Mathf.DegreesToRadians(euler.X) / 2);
        float c3 = Mathf.Cos(Mathf.DegreesToRadians(euler.Z) / 2);
        float s3 = Mathf.Sin(Mathf.DegreesToRadians(euler.Z) / 2);
        return new Quaternion(
            s1 * c2 * c3 + c1 * s2 * s3,
            c1 * s2 * c3 - s1 * c2 * s3,
            c1 * c2 * s3 - s1 * s2 * c3,
            c1 * c2 * c3 + s1 * s2 * s3
        );
    }

    public static Quaternion FromEulerAngles(float x, float y, float z)
    {
        var euler = new Vector3(x, y, z);

        float c1 = Mathf.Cos(Mathf.DegreesToRadians(euler.Y) / 2);
        float s1 = Mathf.Sin(Mathf.DegreesToRadians(euler.Y) / 2);
        float c2 = Mathf.Cos(Mathf.DegreesToRadians(euler.X) / 2);
        float s2 = Mathf.Sin(Mathf.DegreesToRadians(euler.X) / 2);
        float c3 = Mathf.Cos(Mathf.DegreesToRadians(euler.Z) / 2);
        float s3 = Mathf.Sin(Mathf.DegreesToRadians(euler.Z) / 2);
        return new Quaternion(
            s1 * c2 * c3 + c1 * s2 * s3,
            c1 * s2 * c3 - s1 * c2 * s3,
            c1 * c2 * s3 - s1 * s2 * c3,
            c1 * c2 * c3 + s1 * s2 * s3
        );
    }

    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
        float halfAngle = Mathf.DegreesToRadians(angle) / 2;
        float s = Mathf.Sin(halfAngle);
        return new Quaternion(axis.X * s, axis.Y * s, axis.Z * s, Mathf.Cos(halfAngle));
    }

    public static Quaternion Conjugate(Quaternion q)
    {
        return new Quaternion(-q.X, -q.Y, -q.Z, q.W);
    }

    public static Quaternion Inverse(Quaternion q)
    {
        float lengthSq = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        if (lengthSq > 0)
        {
            float invLengthSq = 1.0f / lengthSq;
            return new Quaternion(-q.X * invLengthSq, -q.Y * invLengthSq, -q.Z * invLengthSq, q.W * invLengthSq);
        }
        return Identity;
    }

    public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
    {
        float cosHalfTheta = a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        if (MathF.Abs(cosHalfTheta) >= 1.0f)
            return a;
        float halfTheta = MathF.Acos(cosHalfTheta);
        float sinHalfTheta = MathF.Sqrt(1.0f - cosHalfTheta * cosHalfTheta);
        if (MathF.Abs(sinHalfTheta) < 0.001f)
            return new Quaternion(
                (a.X * 0.5f + b.X * 0.5f),
                (a.Y * 0.5f + b.Y * 0.5f),
                (a.Z * 0.5f + b.Z * 0.5f),
                (a.W * 0.5f + b.W * 0.5f)
            );
        float ratioA = MathF.Sin((1 - t) * halfTheta) / sinHalfTheta;
        float ratioB = MathF.Sin(t * halfTheta) / sinHalfTheta;
        return new Quaternion(
            a.X * ratioA + b.X * ratioB,
            a.Y * ratioA + b.Y * ratioB,
            a.Z * ratioA + b.Z * ratioB,
            a.W * ratioA + b.W * ratioB
        );
    }

    public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
    {
        return new Quaternion(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t,
            a.Z + (b.Z - a.Z) * t,
            a.W + (b.W - a.W) * t
        ).Normalized();
    }

    public static Quaternion LookRotation(Vector3 forward, Vector3 up)
    {
        Vector3 z = forward.Normalized();
        Vector3 x = Vector3.Cross(up, z).Normalized();
        Vector3 y = Vector3.Cross(z, x);
        float m00 = x.X;
        float m01 = y.X;
        float m02 = z.X;
        float m10 = x.Y;
        float m11 = y.Y;
        float m12 = z.Y;
        float m20 = x.Z;
        float m21 = y.Z;
        float m22 = z.Z;
        float num8 = (m00 + m11) + m22;
        Quaternion q = new();
        if (num8 > 0.0f)
        {
            float num = MathF.Sqrt(num8 + 1.0f);
            q.W = num * 0.5f;
            num = 0.5f / num;
            q.X = (m11 - m22) * num;
            q.Y = (m22 - m00) * num;
            q.Z = (m00 - m11) * num;
            return q;
        }
        if ((m00 >= m11) && (m00 >= m22))
        {
            float num7 = MathF.Sqrt(((1.0f + m00) - m11) - m22);
            float num4 = 0.5f / num7;
            q.X = 0.5f * num7;
            q.Y = (m01 + m10) * num4;
            q.Z = (m02 + m20) * num4;
            q.W = (m11 - m22) * num4;
            return q;
        }
        if (m11 > m22)
        {
            float num6 = MathF.Sqrt(((1.0f + m11) - m00) - m22);
            float num3 = 0.5f / num6;
            q.X = (m10 + m01) * num3;
            q.Y = 0.5f * num6;
            q.Z = (m21 + m12) * num3;
            q.W = (m22 - m00) * num3;
            return q;
        }
        else
        {
            float num5 = MathF.Sqrt(((1.0f + m22) - m00) - m11);
            float num2 = 0.5f / num5;
            q.X = (m20 + m02) * num2;
            q.Y = (m21 + m12) * num2;
            q.Z = 0.5f * num5;
            q.W = (m00 - m11) * num2;
            return q;
        }
    }

    public static Quaternion LookRotation(Vector3 forward)
    {
        return LookRotation(forward, Vector3.UnitY);
    }

    public static Quaternion FromToRotation(Vector3 from, Vector3 to)
    {
        Vector3 f = from.Normalized();
        Vector3 t = to.Normalized();
        float cosTheta = Vector3.Dot(f, t);
        Vector3 rotationAxis;
        if (cosTheta < -0.9999f)
        {
            rotationAxis = Vector3.Cross(Vector3.UnitX, f);
            if (rotationAxis.Length() < 0.0001f)
                rotationAxis = Vector3.Cross(Vector3.UnitY, f);
            rotationAxis = rotationAxis.Normalized();
            return FromAxisAngle(rotationAxis, 180);
        }
        rotationAxis = Vector3.Cross(f, t);
        float s = MathF.Sqrt((1 + cosTheta) * 2);
        float invs = 1 / s;
        return new Quaternion(rotationAxis.X * invs, rotationAxis.Y * invs, rotationAxis.Z * invs, s * 0.5f).Normalized();
    }

    public static Quaternion Normalize(Quaternion q) => q.Normalized();

    #endregion

    public override readonly string ToString()
    {
        return $"({X}, {Y}, {Z}, {W})";
    }
}
