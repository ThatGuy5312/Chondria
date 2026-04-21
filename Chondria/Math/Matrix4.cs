namespace Chondria.Math;

// A 4 by 4 matrice holding four Vector4s. Mainly used for transformation matrix.
public struct Matrix4
{
    /// <summary>
    /// Top row of the matrix.
    /// </summary>
    public Vector4 Row0;

    /// <summary>
    /// 2nd row of the matrix.
    /// </summary>
    public Vector4 Row1;

    /// <summary>
    /// 3rd row of the matrix.
    /// </summary>
    public Vector4 Row2;

    /// <summary>
    /// Bottom row of the matrix.
    /// </summary>
    public Vector4 Row3;

    public static Matrix4 Identity => new(1, 0, 0, 0,
                              0, 1, 0, 0,
                              0, 0, 1, 0,
                              0, 0, 0, 1);

    public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    public Matrix4(float m00, float m01, float m02, float m03,
                   float m10, float m11, float m12, float m13,
                   float m20, float m21, float m22, float m23,
                   float m30, float m31, float m32, float m33)
    {
        Row0 = new Vector4(m00, m01, m02, m03);
        Row1 = new Vector4(m10, m11, m12, m13);
        Row2 = new Vector4(m20, m21, m22, m23);
        Row3 = new Vector4(m30, m31, m32, m33);
    }

    #region operators

    public static implicit operator OpenTK.Mathematics.Matrix4(Matrix4 m) => new(m.Row0, m.Row1, m.Row2, m.Row3);
    public static implicit operator Matrix4(OpenTK.Mathematics.Matrix4 m) => new(m.Row0, m.Row1, m.Row2, m.Row3);

    public static Matrix4 operator *(Matrix4 a, Matrix4 b)
    {
        var result = new Matrix4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = a[i, 0] * b[0, j] + a[i, 1] * b[1, j] + a[i, 2] * b[2, j] + a[i, 3] * b[3, j];
            }
        }
        return result;
    }

    public static Vector4 operator *(Matrix4 m, Vector4 v)
    {
        return new Vector4(
            m.Row0.X * v.X + m.Row0.Y * v.Y + m.Row0.Z * v.Z + m.Row0.W * v.W,
            m.Row1.X * v.X + m.Row1.Y * v.Y + m.Row1.Z * v.Z + m.Row1.W * v.W,
            m.Row2.X * v.X + m.Row2.Y * v.Y + m.Row2.Z * v.Z + m.Row2.W * v.W,
            m.Row3.X * v.X + m.Row3.Y * v.Y + m.Row3.Z * v.Z + m.Row3.W * v.W
        );
    }

    public static Matrix4 operator *(Matrix4 a, OpenTK.Mathematics.Matrix4 b)
    {
        var result = new Matrix4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = a[i, 0] * b[0, j] + a[i, 1] * b[1, j] + a[i, 2] * b[2, j] + a[i, 3] * b[3, j];
            }
        }
        return result;
    }

    public static Vector4 operator *(Matrix4 m, OpenTK.Mathematics.Vector4 v)
    {
        return new Vector4(
            m.Row0.X * v.X + m.Row0.Y * v.Y + m.Row0.Z * v.Z + m.Row0.W * v.W,
            m.Row1.X * v.X + m.Row1.Y * v.Y + m.Row1.Z * v.Z + m.Row1.W * v.W,
            m.Row2.X * v.X + m.Row2.Y * v.Y + m.Row2.Z * v.Z + m.Row2.W * v.W,
            m.Row3.X * v.X + m.Row3.Y * v.Y + m.Row3.Z * v.Z + m.Row3.W * v.W
        );
    }

    public float this[int row, int column]
    {
        get
        {
            return row switch
            {
                0 => Row0[column],
                1 => Row1[column],
                2 => Row2[column],
                3 => Row3[column],
                _ => throw new IndexOutOfRangeException("Row index must be between 0 and 3.")
            };
        }
        set
        {
            switch (row)
            {
                case 0: Row0[column] = value; break;
                case 1: Row1[column] = value; break;
                case 2: Row2[column] = value; break;
                case 3: Row3[column] = value; break;
                default: throw new IndexOutOfRangeException("Row index must be between 0 and 3.");
            }
        }
    }

    #endregion

    #region static functions

    public static Matrix4 CreateTranslation(Vector3 translation)
    {
        var result = Identity;

        result.Row3.X = translation.X;
        result.Row3.Y = translation.Y;
        result.Row3.Z = translation.Z;

        return result;
    }

    public static Matrix4 CreateScale(Vector3 scale)
    {
        return new Matrix4(scale.X, 0, 0, 0,
                           0, scale.Y, 0, 0,
                           0, 0, scale.Z, 0,
                           0, 0, 0, 1);
    }

    public static Matrix4 CreateFromQuaternion(Quaternion rotation)
    {
        float x = rotation.X, y = rotation.Y, z = rotation.Z, w = rotation.W;
        return new Matrix4(
            1 - 2 * (y * y + z * z), 2 * (x * y - z * w), 2 * (x * z + y * w), 0,
            2 * (x * y + z * w), 1 - 2 * (x * x + z * z), 2 * (y * z - x * w), 0,
            2 * (x * z - y * w), 2 * (y * z + x * w), 1 - 2 * (x * x + y * y), 0,
            0, 0, 0, 1);
    }

    public static Matrix4 CreateRotationX(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Matrix4(1, 0, 0, 0,
                           0, cos, -sin, 0,
                           0, sin, cos, 0,
                           0, 0, 0, 1);
    }

    public static Matrix4 CreateRotationY(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Matrix4(cos, 0, sin, 0,
                           0, 1, 0, 0,
                           -sin, 0, cos, 0,
                           0, 0, 0, 1);
    }

    public static Matrix4 CreateRotationZ(float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Matrix4(cos, -sin, 0, 0,
                           sin, cos, 0, 0,
                           0, 0, 1, 0,
                           0, 0, 0, 1);
    }

    public static Matrix4 CreatePerspectiveFieldOfView(float fov, float aspectRatio, float near, float far)
    {
        float yScale = 1.0f / (float)Mathf.Tan(fov / 2);
        float xScale = yScale / aspectRatio;
        float zScale = far / (far - near);
        return new Matrix4(xScale, 0, 0, 0,
                           0, yScale, 0, 0,
                           0, 0, zScale, -near * zScale,
                           0, 0, 1, 0);
    }

    public static Matrix4 CreateOrthographic(float left, float right, float bottom, float top, float near, float far)
    {
        return new Matrix4(2 / (right - left), 0, 0, -(right + left) / (right - left),
                           0, 2 / (top - bottom), 0, -(top + bottom) / (top - bottom),
                           0, 0, -2 / (far - near), -(far + near) / (far - near),
                           0, 0, 0, 1);
    }

    public static Matrix4 CreateLookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        Vector3 zAxis = (eye - target).Length() == 0 ? Vector3.UnitZ : (eye - target) / (eye - target).Length();
        Vector3 xAxis = Vector3.Cross(up, zAxis).Length() == 0 ? Vector3.UnitX : Vector3.Cross(up, zAxis) / Vector3.Cross(up, zAxis).Length();
        Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
        return new Matrix4(xAxis.X, yAxis.X, zAxis.X, eye.X,
                           xAxis.Y, yAxis.Y, zAxis.Y, eye.Y,
                           xAxis.Z, yAxis.Z, zAxis.Z, eye.Z,
                           0, 0, 0, 1);
    }

    public static Vector3 ExtractTranslation(Matrix4 matrix)
    {
        return new Vector3(matrix.Row3.X, matrix.Row3.Y, matrix.Row3.Z);
    }

    public static Quaternion ExtractRotation(Matrix4 matrix)
    {
        float trace = matrix.Row0.X + matrix.Row1.Y + matrix.Row2.Z;
        if (trace > 0)
        {
            float s = 0.5f / Mathf.Sqrt(trace + 1.0f);
            return new Quaternion(
                (matrix.Row1.Z - matrix.Row2.Y) * s,
                (matrix.Row2.X - matrix.Row0.Z) * s,
                (matrix.Row0.Y - matrix.Row1.X) * s,
                0.25f / s
            );
        }
        else
        {
            if (matrix.Row0.X > matrix.Row1.Y && matrix.Row0.X > matrix.Row2.Z)
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + matrix.Row0.X - matrix.Row1.Y - matrix.Row2.Z);
                return new Quaternion(
                    0.25f * s,
                    (matrix.Row0.Y + matrix.Row1.X) / s,
                    (matrix.Row0.Z + matrix.Row2.X) / s,
                    (matrix.Row1.Z - matrix.Row2.Y) / s
                );
            }
            else if (matrix.Row1.Y > matrix.Row2.Z)
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + matrix.Row1.Y - matrix.Row0.X - matrix.Row2.Z);
                return new Quaternion(
                    (matrix.Row0.Y + matrix.Row1.X) / s,
                    0.25f * s,
                    (matrix.Row1.Z + matrix.Row2.Y) / s,
                    (matrix.Row2.X - matrix.Row0.Z) / s
                );
            }
            else
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + matrix.Row2.Z - matrix.Row0.X - matrix.Row1.Y);
                return new Quaternion(
                    (matrix.Row0.Z + matrix.Row2.X) / s,
                    (matrix.Row1.Z + matrix.Row2.Y) / s,
                    0.25f * s,
                    (matrix.Row0.Y - matrix.Row1.X) / s
                );
            }
        }
    }

    public static Vector3 ExtractScale(Matrix4 matrix)
    {
        float scaleX = new Vector3(matrix.Row0.X, matrix.Row0.Y, matrix.Row0.Z).Length();
        float scaleY = new Vector3(matrix.Row1.X, matrix.Row1.Y, matrix.Row1.Z).Length();
        float scaleZ = new Vector3(matrix.Row2.X, matrix.Row2.Y, matrix.Row2.Z).Length();
        return new Vector3(scaleX, scaleY, scaleZ);
    }

    #endregion

    #region instance functions

    public Vector3 ExtractTranslation() => ExtractTranslation(this);

    public Quaternion ExtractRotation() => ExtractRotation(this);
    public Vector3 ExtractEulerRotation() => ExtractRotation(this).ToEulerAngles();

    public Vector3 ExtractScale() => ExtractScale(this);

    public void LookAt(Vector3 target, Vector3 up)
    {
        var lookAtMatrix = CreateLookAt(ExtractTranslation(), target, up);
        Row0 = lookAtMatrix.Row0;
        Row1 = lookAtMatrix.Row1;
        Row2 = lookAtMatrix.Row2;
    }

    #endregion

    public override string ToString()
    {
        return $"[{Row0.X}, {Row0.Y}, {Row0.Z}, {Row0.W}]\n" +
               $"[{Row1.X}, {Row1.Y}, {Row1.Z}, {Row1.W}]\n" +
               $"[{Row2.X}, {Row2.Y}, {Row2.Z}, {Row2.W}]\n" +
               $"[{Row3.X}, {Row3.Y}, {Row3.Z}, {Row3.W}]";
    }
}
