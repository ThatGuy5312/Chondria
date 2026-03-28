using Chondria.Rendering.Drawers;

namespace Chondria.Rendering
{
    // test drawer/pipeline
    internal class TestPatternDrawer : IDrawer
    {
        public void Draw(PixelBuffer buffer)
        {
            buffer.Clear(0, 0, 0, 255);

            for (int y = 0; y < buffer.Height; y++)
                for (int x = 0; x < buffer.Width; x++)
                {
                    byte color = (byte)((x + y + (int)(DateTime.Now.TimeOfDay.TotalMilliseconds * 0.1)) % 255);
                    buffer.SetPixel(x, y, color, 0, 255, 255);
                }
        }
    }
}
