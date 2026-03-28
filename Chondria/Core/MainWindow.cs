using Chondria.Rendering;
using Chondria.Rendering.Drawers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Chondria.Core;

// main window for everything
internal class MainWindow : GameWindow
{
    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    private Renderer renderer;

    // temperary but kinda like a render pipeline
    private IDrawer drawer;

    protected override void OnLoad()
    {
        base.OnLoad();

        renderer = new Renderer();
        renderer.Init(Size.X, Size.Y);

        drawer = new TestPatternDrawer();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        renderer.Resize(Size.X, Size.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        // This is where drawing happens
        GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // passing data instead of having the renderer do this
        drawer.Draw(renderer.Buffer);

        renderer.Render();

        SwapBuffers();
    }
}
