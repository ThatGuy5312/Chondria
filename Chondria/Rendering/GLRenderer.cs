using Chondria.Management;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection.Metadata;

namespace Chondria.Rendering;

// the main renderer and powerhouse of the rendering system, handles all the rendering and shader management
// renderer version 2026.0.2.1
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

        shader.SetInt("lightCount", LightingSettings.Lights.Count);

        for (int i = 0; i < LightingSettings.Lights.Count; i++)
        {
            var light = LightingSettings.Lights[i];

            string prefix = $"lights[{i}]";

            shader.SetVector3(prefix + ".position", light.Position);
            shader.SetVector3(prefix + ".color", light.Color);

            shader.SetFloat(prefix + ".constant", light.Constant);
            shader.SetFloat(prefix + ".linear", light.Linear);
            shader.SetFloat(prefix + ".quadratic", light.Quadratic);
        }

        shader.SetVector3("objectColor", LightingSettings.Material.Color);
        shader.SetFloat("specularStrength", LightingSettings.Material.SpecularStrength);
        shader.SetFloat("shininess", LightingSettings.Material.Shininess);
        shader.SetVector3("viewPos", camera.Position);

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
#define MAX_LIGHTS 8

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

struct Light
{
    vec3 position;
    vec3 color;

    float constant;
    float linear;
    float quadratic;
};

uniform Light lights[MAX_LIGHTS];
uniform int lightCount;

uniform vec3 viewPos;
uniform vec3 objectColor;

uniform float specularStrength;
uniform float shininess;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    vec3 result = vec3(0.0);

    for (int i = 0; i < lightCount; i++)
    {
        vec3 lightDir = normalize(lights[i].position - FragPos);

        // Diffuse
        float diff = max(dot(norm, lightDir), 0.0);

        // Specular
        vec3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);

        // Attenuation
        float distance = length(lights[i].position - FragPos);
        float attenuation = 1.0 / (
            lights[i].constant +
            lights[i].linear * distance +
            lights[i].quadratic * distance * distance
        );

        vec3 diffuse = diff * lights[i].color * objectColor;
        vec3 specular = specularStrength * spec * lights[i].color;

        diffuse *= attenuation;
        specular *= attenuation;

        result += diffuse + specular;
    }

    FragColor = vec4(result, 1.0);
}";
}

public static class LightingSettings
{
    public static List<Light> Lights = new()
    {
        new Light
        {
            Position = new(2f, 2f, 2f),
            Color = new(1f, 1f, 1f),
            Constant = 1f,
            Linear = 0.09f,
            Quadratic = 0.032f
        },
        new Light
        {
            Position = new(-2f, 1f, 0f),
            Color = new(0.2f, 0.5f, 1f),
            Constant = 1f,
            Linear = 0.09f,
            Quadratic = 0.032f
        }
    };

    public static Material Material = new Material
    {
        Color = new(1f, 0.5f, 0.3f),
        SpecularStrength = 0.5f,
        Shininess = 32f
    };
}

public struct Light
{
    public Vector3 Position;
    public Vector3 Color;

    public float Constant;
    public float Linear;
    public float Quadratic;
}
