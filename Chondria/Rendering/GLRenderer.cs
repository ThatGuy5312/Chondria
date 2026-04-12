using Chondria.Scene;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection.Metadata;

namespace Chondria.Rendering;

// the main renderer and powerhouse of the rendering system, handles all the rendering and shader management
// renderer version 2026.0.1.0
public class GLRenderer
{
    int vao;
    int vbo;
    Shader shader;

    public void Init()
    {
        GL.Enable(EnableCap.DepthTest);

        float[] vertices = {
            // position        // normal
            -0.5f, -0.5f, 0.5f,  0, 0, 1,
             0.5f, -0.5f, 0.5f,  0, 0, 1,
             0.5f,  0.5f, 0.5f,  0, 0, 1,

            0.5f,  0.5f, 0.5f,  0, 0, 1,
            -0.5f,  0.5f, 0.5f,  0, 0, 1,
            -0.5f, -0.5f, 0.5f,  0, 0, 1,

            -0.5f, -0.5f, -0.5f,  0, 0, -1,
            0.5f, -0.5f, -0.5f,  0, 0, -1,
            0.5f, 0.5f, -0.5f,  0, 0, -1,

            0.5f, 0.5f, -0.5f,  0, 0, -1,
            -0.5f, 0.5f, -0.5f,  0, 0, -1,
            -0.5f, -0.5f, -0.5f,  0, 0, -1,

            -0.5f, 0.5f, 0.5f,  -1, 0, 0,
            -0.5f, 0.5f, -0.5f,  -1, 0, 0,
            -0.5f, -0.5f, -0.5f,  -1, 0, 0,

            -0.5f, -0.5f, -0.5f,  -1, 0, 0,
            -0.5f, -0.5f, 0.5f,  -1, 0, 0,
            -0.5f, 0.5f, 0.5f,  -1, 0, 0,

            0.5f, 0.5f, 0.5f,  1, 0, 0,
            0.5f, 0.5f, -0.5f,  1, 0, 0,
            0.5f, -0.5f, -0.5f,  1, 0, 0,

            0.5f, -0.5f, -0.5f,  1, 0, 0,
            0.5f, -0.5f, 0.5f,  1, 0, 0,
            0.5f, 0.5f, 0.5f,  1, 0, 0,

            -0.5f, 0.5f, -0.5f,  0, 1, 0,
            0.5f, 0.5f, -0.5f,  0, 1, 0,
            0.5f, 0.5f, 0.5f,  0, 1, 0,

            0.5f, 0.5f, 0.5f,  0, 1, 0,
            -0.5f, 0.5f, 0.5f,  0, 1, 0,
            -0.5f, 0.5f, -0.5f,  0, 1, 0,

            -0.5f, -0.5f, -0.5f,  0, -1, 0,
            0.5f, -0.5f, -0.5f,  0, -1, 0,
            0.5f, -0.5f, 0.5f,  0, -1, 0,

            0.5f, -0.5f, 0.5f,  0, -1, 0,
            -0.5f, -0.5f, 0.5f,  0, -1, 0,
            -0.5f, -0.5f, -0.5f,  0, -1, 0,
        };

        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        int stride = 6 * sizeof(float);

        // position
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        // normal
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        shader = new Shader(vertexSource, fragmentSource);
    }

    public void Render(in Camera camera)
    {
        //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var model = Matrix4.CreateScale(1, 1f, 1) *
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0)) *
        Matrix4.CreateRotationY((float)DateTime.Now.TimeOfDay.TotalSeconds) *
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(0)) *
        Matrix4.CreateTranslation(0, 0, 0);

        shader.Use();
        shader.SetMatrix4("model", model);
        shader.SetMatrix4("view", camera.View);
        shader.SetMatrix4("projection", camera.Projection);

        GL.Uniform3(GL.GetUniformLocation(shader.Handle, "lightPos"), LightingSettings.LightPos);
        GL.Uniform3(GL.GetUniformLocation(shader.Handle, "objectColor"), LightingSettings.ObjectColor);
        GL.Uniform1(GL.GetUniformLocation(shader.Handle, "specularStrength"), LightingSettings.SpecularStrength);
        GL.Uniform1(GL.GetUniformLocation(shader.Handle, "shininess"), LightingSettings.Shininess);
        GL.Uniform3(GL.GetUniformLocation(shader.Handle, "viewPos"), camera.Position);

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    public void Resize(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
    }

    string vertexSource = @"
#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;

void main()
{
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal;

    gl_Position = projection * view * vec4(FragPos, 1.0);
}";

    string fragmentSource = @"
#version 330 core

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 objectColor;

uniform float specularStrength;
uniform float shininess;

void main()
{
    vec3 norm = normalize(Normal);

    // Light direction (point light!)
    vec3 lightDir = normalize(lightPos - FragPos);

    // Diffuse
    float diff = max(dot(norm, lightDir), 0.0);

    // View direction
    vec3 viewDir = normalize(viewPos - FragPos);

    // Reflection
    vec3 reflectDir = reflect(-lightDir, norm);

    // Specular
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

    float distance = length(lightPos - FragPos);

    float constant = 1.0;
    float linear = 0.09;
    float quadratic = 0.032;

    float attenuation = 1.0 / (constant + linear * distance + quadratic * (distance * distance));

    vec3 diffuse = diff * objectColor;
    vec3 specular = specularStrength * spec * vec3(1.0);

    diffuse *= attenuation;
    specular *= attenuation;

    vec3 result = diffuse + specular;

    FragColor = vec4(result, 1.0);
}";
}

public static class LightingSettings
{
    public static Math.Vector3 LightPos = new Vector3(2f, 2f, 2f);

    public static Math.Vector3 ObjectColor = new Vector3(1f, 0.5f, 0.3f);

    public static float SpecularStrength = 0.5f;
    public static float Shininess = 32f;
}
