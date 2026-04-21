using Chondria.Math;
using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering;

// creates a shader program from provided vertex and fragment shader source code
public class Shader
{
    static Shader()
    {
        DefaultLit = new Shader(litVertexSource, litFragmentSource);
    }

    public int Handle;

    public Shader(string vertexSource, string fragmentSource)
    {
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);

        BuildShader(vertexShader, fragmentShader);
    }

    public Shader(string combinedSource)
    {
        string vertexSource = string.Empty;
        string fragmentSource = string.Empty;

        ParseCombinedSource(combinedSource, out vertexSource, out fragmentSource);

        int vertex = CompileShader(ShaderType.VertexShader, vertexSource);
        int fragment = CompileShader(ShaderType.FragmentShader, fragmentSource);

        BuildShader(vertex, fragment);
    }

    int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
            throw new Exception($"{type} compilation failed: {GL.GetShaderInfoLog(shader)}");
        return shader;
    }

    void ParseCombinedSource(string source, out string vertexSrc, out string fragmentSrc)
    {
        var lines = source.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var currentType = string.Empty;

        var vertex = new System.Text.StringBuilder();
        var fragment = new System.Text.StringBuilder();

        foreach (string line in lines)
        {
            if (line.StartsWith("#shader"))
            {
                if (line.Contains("vertex")) currentType = "vertex";
                else if (line.Contains("fragment")) currentType = "fragment";
            }
            else
            {
                if (currentType == "vertex") vertex.AppendLine(line);
                else if (currentType == "fragment") fragment.AppendLine(line);
            }
        }

        vertexSrc = vertex.ToString();
        fragmentSrc = fragment.ToString();
    }

    void BuildShader(int vertexShader, int fragmentShader)
    {
        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use() => GL.UseProgram(Handle);

    public void SetMatrix4(string name, Matrix4 value)
    {
        var tkvalue = value.TK();
        GL.UniformMatrix4(GL.GetUniformLocation(Handle, name), false, ref tkvalue);
    }

    public void SetVector3(string name, Vector3 value)
        => GL.Uniform3(GL.GetUniformLocation(Handle, name), value);

    public void SetVector2(string name, Vector2 value)
        => GL.Uniform2(GL.GetUniformLocation(Handle, name), value);

    public void SetVector4(string name, Vector4 value)
        => GL.Uniform4(GL.GetUniformLocation(Handle, name), value);

    public void SetInt(string name, int value)
        => GL.Uniform1(GL.GetUniformLocation(Handle, name), value);

    public void SetFloat(string name, float value)
        => GL.Uniform1(GL.GetUniformLocation(Handle, name), value);

    public static Shader DefaultLit;

    static string litVertexSource = @"
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

    static string litFragmentSource = @"
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

        vec3 ambient = 0.1 * lights[i].color * objectColor;
        vec3 diffuse = diff * lights[i].color * objectColor;
        vec3 specular = specularStrength * spec * lights[i].color;

        ambient *= attenuation;
        diffuse *= attenuation;
        specular *= attenuation;

        result += ambient + diffuse + specular;
    }

    if (lightCount == 0)
    {
        result = vec3(objectColor);
    }

    FragColor = vec4(result, 1.0);
}";
}
