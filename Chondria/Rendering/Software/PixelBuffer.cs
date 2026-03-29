using Chondria.Math;
using OpenTK.Graphics.OpenGL4;

namespace Chondria.Rendering.Software;

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

    public void DrawRect(int startX, int startY, int width, int height, byte r, byte g, byte b, byte a)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                SetPixel(startX + x, startY + y, r, g, b, a);
            }
    }

    public void DrawLine(int x0, int y0, int x1, int y1, byte r, byte g, byte b, byte a)
    {
        int dx = System.Math.Abs(x1 - x0);
        int dy = System.Math.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int err = dx - dy;

        while (true)
        {
            SetPixel(x0, y0, r, g, b, a);

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    public void FillTriangle(Vector2 v0, Vector2 v1, Vector2 v2, byte r, byte g, byte b, byte a)
    {
        // bounding box
        int minX = (int)System.Math.Min(v0.X, System.Math.Min(v1.X, v2.X));
        int maxX = (int)System.Math.Max(v0.X, System.Math.Max(v1.X, v2.X));
        int minY = (int)System.Math.Min(v0.Y, System.Math.Min(v1.Y, v2.Y));
        int maxY = (int)System.Math.Max(v0.Y, System.Math.Max(v1.Y, v2.Y));

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (PointInTriangle(x, y, v0, v1, v2))
                {
                    SetPixel(x, y, r, g, b, a);
                }
            }
        }
    }

    private bool PointInTriangle(int px, int py, Vector2 v0, Vector2 v1, Vector2 v2)
    {
        float dX = px - v2.X;
        float dY = py - v2.Y;

        float dX21 = v2.X - v1.X;
        float dY12 = v1.Y - v2.Y;
        float D = dY12 * (v0.X - v2.X) + dX21 * (v0.Y - v2.Y);

        float s = dY12 * dX + dX21 * dY;
        float t = (v2.Y - v0.Y) * dX + (v0.X - v2.X) * dY;

        if (D < 0)
            return s <= 0 && t <= 0 && s + t >= D;
        return s >= 0 && t >= 0 && s + t <= D;
    }

    public Vector2 Project(Vector3 v, float fov, float width, float height)
    {
        float z = v.Z;

        if (z < 0.1f)
            z = 0.1f;

        float factor = fov / z;

        return new Vector2(
            v.X * factor + width / 2,
            v.Y * factor + height / 2
        );
    }

    public void DrawTriangle(Vector2 p0, Vector2 p1, Vector2 p2, byte r, byte g, byte b, byte a)
    {
        DrawLine((int)p0.X, (int)p0.Y, (int)p1.X, (int)p1.Y, r, g, b, a);
        DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, r, g, b, a);
        DrawLine((int)p2.X, (int)p2.Y, (int)p0.X, (int)p0.Y, r, g, b, a);
    }

    public void Upload()
    {
        GL.BindTexture(TextureTarget.Texture2D, Texture);
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height,
            PixelFormat.Rgba, PixelType.UnsignedByte, data);
    }
}
