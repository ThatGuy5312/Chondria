namespace Chondria.Rendering.Drawers;

// kinda like a render pipeline, temperary
public interface IDrawer
{
    void Draw(PixelBuffer buffer);
}
