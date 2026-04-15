using Chondria.Math;

namespace Chondria.Entities
{
    public class Transform
    {
        public MeshRenderer? Entity { get; set; }

        public Transform? Parent { get; set; }

        public List<Transform> Children { get; set; } = [];

        public Vector3 Position { get; set; } = Vector3.Zero;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Vector3 Scale { get; set; } = Vector3.One;

        private Matrix4 m_LocalMatrix =>
            Matrix4.CreateScale(Scale) *
            Matrix4.CreateFromQuaternion(Rotation) *
            Matrix4.CreateTranslation(Position);

        internal Matrix4 Matrix
        {
            get
            {
                if (Parent != null)
                    return m_LocalMatrix * Parent.Matrix;
                else
                    return m_LocalMatrix;
            }
        }

        public void AddChild(Transform child)
        {
            child.Parent = this;
            Children.Add(child);
        }
    }
}
