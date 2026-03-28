using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering;

// basically creates a image based off of provided pixel data
public class PixelBuffer
{
    public int Texture;
    public int Width;
    public int Height;
    private byte[] data;

    public PixelBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        data = new byte[width * height * 4];

        Texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Texture);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
    }

    public void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return;

        int index = (y * Width + x) * 4;
        data[index + 0] = r;
        data[index + 1] = g;
        data[index + 2] = b;
        data[index + 3] = a;
    }

    public void Clear(byte r, byte g, byte b, byte a)
    {
        for (int i = 0; i < data.Length; i += 4)
        {
            data[i + 0] = r;
            data[i + 1] = g;
            data[i + 2] = b;
            data[i + 3] = a;
        }
    }

    public void Upload()
    {
        GL.BindTexture(TextureTarget.Texture2D, Texture);
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height,
            PixelFormat.Rgba, PixelType.UnsignedByte, data);
    }
}
