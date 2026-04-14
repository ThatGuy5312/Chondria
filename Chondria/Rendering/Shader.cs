using Chondria.Math;
using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering;

// creates a shader program from provided vertex and fragment shader source code
public class Shader
{
    public int Handle;

    public Shader(string vertexSource, string fragmentSource)
    {
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        GL.CompileShader(fragmentShader);

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
}
