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

    public void Use()
    {
        GL.UseProgram(Handle);
    }
}
