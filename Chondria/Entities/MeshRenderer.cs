using Chondria.Rendering;

namespace Chondria.Entities
{
    public class MeshRenderer
    {
        public string Name = "MeshRenderer";

        public Transform Transform = new Transform();
        public Material Material = new Material();
        public Mesh Mesh;

        public MeshRenderer(Mesh mesh)
        {
            Mesh = mesh;
            Transform.Entity = this;
        }

        public void Render(Shader shader)
        {
            shader.SetMatrix4("model", Transform.Matrix);
            Material.Apply(shader);
        }
    }
}
