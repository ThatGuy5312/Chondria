using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering;

// just a start, this will eventually be used to render certain pipelines and turning them into a image to display
public class Renderer
{
    private Shader shader;
    private FullscreenQuad quad;
    private PixelBuffer buffer;

    public PixelBuffer Buffer => buffer;

    public void Init(int width, int height)
    {
        shader = new Shader(
            @"#version 330 core
        layout (location = 0) in vec2 aPos;
        layout (location = 1) in vec2 aUV;
        out vec2 uv;
        void main()
        {
            uv = aUV;
            gl_Position = vec4(aPos, 0.0, 1.0);
        }",
            @"#version 330 core
        out vec4 FragColor;
        in vec2 uv;
        uniform sampler2D screenTexture;
        void main()
        {
            FragColor = texture(screenTexture, uv);
        }"
        );

        quad = new FullscreenQuad();
        Resize(width, height);
    }

    public void Resize(int width, int height)
    {
        buffer = new PixelBuffer(width, height);

        GL.Viewport(0, 0, width, height);
    }

    public void Render()
    {
        buffer.Upload();

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        GL.BindTexture(TextureTarget.Texture2D, buffer.Texture);

        quad.Draw();
    }
}
