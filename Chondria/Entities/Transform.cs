using Chondria.Math;

namespace Chondria.Entities
{
    public class Transform
    {
        public Vector3 Position { get; set; } = Vector3.Zero;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Vector3 Scale { get; set; } = Vector3.One;

        internal Matrix4 Matrix =>
            Matrix4.CreateScale(Scale) *
            Matrix4.CreateFromQuaternion(Rotation) *
            OpenTK.Mathematics.Matrix4.CreateTranslation(Position); // ill fix it later
        //Matrix4.CreateTranslation(Position);
    }
}
