using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering
{
    public class Mesh
    {
        public int VAO;
        public int VBO;

        public Mesh() { }

        public Mesh(float[] vertices)
        {
            var mesh = Create(vertices);

            VAO = mesh.VAO;
            VBO = mesh.VBO;
        }

        public static Mesh Create(float[] vertices)
        {
            var mesh = new Mesh();

            mesh.VAO = GL.GenVertexArray();
            mesh.VBO = GL.GenBuffer();
            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            return mesh;
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}
