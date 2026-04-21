using Chondria.Entities;
using Chondria.Management;
using Chondria.Math;
using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering;

// the main renderer and powerhouse of the rendering system, handles all the rendering and shader management
// renderer version 2026.0.4.1
public class GLRenderer
{
    List<Light> m_Lights = [];

    public void Init()
    {
        GL.Enable(EnableCap.DepthTest);
    }

    public void Render(in Scene scene, in Camera camera)
    {
        //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        m_Lights.Clear();

        for (int i = 0; i < scene.Objects.Length; i++)
        {
            var obj = scene.Objects[i];

            if (obj.GetComponent<Light>() != null)
            {
                var light = obj.GetComponent<Light>();

                m_Lights.Add(light);
            }
        }
        
        for (int i = 0; i < scene.Objects.Length; i++)
        {
            var obj = scene.Objects[i];

            if (obj.GetComponent<MeshRenderer>() != null)
            {
                var mr = obj.GetComponent<MeshRenderer>();

                var shader = mr.Material.Shader;

                shader.Use();
                mr.Render(shader);

                shader.SetMatrix4("view", camera.View);
                shader.SetMatrix4("projection", camera.Projection);

                shader.SetInt("lightCount", m_Lights.Count);

                for (int j = 0; j < m_Lights.Count; j++)
                {
                    var light = m_Lights[j];

                    var prefix = $"lights[{j}]";

                    shader.SetVector3(prefix + ".position", light.Transform.Position);
                    shader.SetVector3(prefix + ".color", light.Color);

                    shader.SetFloat(prefix + ".constant", light.Constant);
                    shader.SetFloat(prefix + ".linear", light.Linear);
                    shader.SetFloat(prefix + ".quadratic", light.Quadratic);
                }

                shader.SetVector3("objectColor", mr.Material.Color);
                shader.SetFloat("specularStrength", mr.Material.SpecularStrength);
                shader.SetFloat("shininess", mr.Material.Shininess);
                shader.SetVector3("viewPos", camera.Position);

                mr.Mesh.Draw();
            }
        }
    }

    public void Resize(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }
}