using Chondria.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chondria.Entities
{
    public class MeshRenderer
    {
        public Transform Transform = new Transform();
        public Material Material = new Material();
        public Mesh Mesh;

        public MeshRenderer(Mesh mesh)
        {
            Mesh = mesh;
        }

        public void Render(Shader shader)
        {
            shader.SetMatrix4("model", Transform.Matrix);
            Material.Apply(shader);
        }
    }
}
