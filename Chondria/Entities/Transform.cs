using Chondria.Math;
using Chondria.Windowing;

namespace Chondria.Entities
{
    // main transformations class that hold data for Position, Rotatio, Scale, Parent, Children, Owner Entity, and Matrix
    public class Transform : Component // yes this counts as a Component because later on when you have stuff like                             
    {                                  // RectTransform or other things like that you will need to be able to switch them out.
        public Transform() { Name = "Transform"; }

        [DisableInspector]
        private Transform? _parent;

        [DisableInspector]
        public Transform? Parent
        {
            get => _parent;
            set
            {
                SetTransform();
                _parent = value;
            }
        }

        [DisableInspector]
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

        void SetTransform()
        {
            Position = Matrix.ExtractTranslation();
            Rotation = Matrix.ExtractRotation();
            Scale = Matrix.ExtractScale();
        }

        public void AddChild(Transform child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void SetParent(Transform parent) => Parent = parent;
    }
}
