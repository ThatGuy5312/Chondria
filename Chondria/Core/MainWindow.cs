using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Scene;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Chondria.Core;

// main window for everything
internal class MainWindow : GameWindow
{
    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    // old things
    //private Renderer renderer;
    // temperary but kinda like a render pipeline
    //private IDrawer drawer;

    private GLRenderer glRenderer;
    private Camera SceneCamera;

    private CameraController cameraController;

    protected override void OnLoad()
    {
        base.OnLoad();

        //renderer = new Renderer();
        //renderer.Init(Size.X, Size.Y);

        //drawer = new TestPatternDrawer();

        SceneCamera = new(new(0, 0, 3), Size.X / (float)Size.Y);

        cameraController = new(SceneCamera);

        glRenderer = new GLRenderer();
        glRenderer.Init();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Input.UpdateInput(MouseState, KeyboardState);

        cameraController.Update((float)args.Time);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        glRenderer.Resize(Size.X, Size.Y);
        //renderer.Resize(Size.X, Size.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // passing data instead of having the renderer do this
        //drawer.Draw(renderer.Buffer);
        //renderer.Render();

        // renders the the cube while passing in a camera.
        // I plan to eventually pass in a scene and camera but this is just a test for now.
        glRenderer.Render(in SceneCamera);

        SwapBuffers();
    }
}
