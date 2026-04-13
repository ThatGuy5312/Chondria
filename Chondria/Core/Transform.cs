using OpenTK.Mathematics;

namespace Chondria.Core;

public class Transform
{
    public Vector3 Position = Vector3.Zero;
    public Quaternion Rotation = Quaternion.Identity;
    public Vector3 Scale = Vector3.One;

    internal Matrix4 Matrix
    {
        get
        {
            var matrix = Matrix4.CreateScale(Scale) *
                     Matrix4.CreateFromQuaternion(Rotation) *
                     Matrix4.CreateTranslation(Position);

            return matrix;
        }
    }
}
