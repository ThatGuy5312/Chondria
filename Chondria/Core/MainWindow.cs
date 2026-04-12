using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Scene;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Chondria.Core;

// main window for everything
internal class MainWindow : GameWindow
{
    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    // old things
    //private Renderer renderer;
    // temperary but kinda like a render pipeline
    //private IDrawer drawer;

    private ImGuiController imGuiController;

    private GLRenderer glRenderer;
    private Camera SceneCamera;

    private CameraController cameraController;

    protected override void OnLoad()
    {
        base.OnLoad();

        //renderer = new Renderer();
        //renderer.Init(Size.X, Size.Y);

        //drawer = new TestPatternDrawer();

        SetupFramebuffer(Size.X, Size.Y);

        imGuiController = new(this);

        SceneCamera = new(new(0, 0, 3), Size.X / (float)Size.Y);

        cameraController = new(SceneCamera);

        glRenderer = new GLRenderer();
        glRenderer.Init();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Time.Update((float)args.Time);

        Input.UpdateInput(MouseState, KeyboardState);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        SetupFramebuffer(e.Width, e.Height);
        glRenderer.Resize(e.Width, e.Height);
        //renderer.Resize(Size.X, Size.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        imGuiController.Update((float)args.Time);

        //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // passing data instead of having the renderer do this
        //drawer.Draw(renderer.Buffer);
        //renderer.Render();

        RenderDockspace();

        //glRenderer.Render(in SceneCamera);
        RenderScene();

        RenderInspector();

        imGuiController.Render();

        GL.Disable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);

        SwapBuffers();
    }

    void RenderScene()
    {
        ImGui.Begin("Scene");

        var sceneSize = ImGui.GetContentRegionAvail();
        this.sceneSize = sceneSize;
        var sceneFocused = ImGui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows);
        if (sceneFocused)
            cameraController.Update(Time.DeltaTime);

        SceneCamera.AspectRatio = sceneSize.X / sceneSize.Y;

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, sceneFBO);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.DepthTest);

        // renders the the cube while passing in a camera.
        // I plan to eventually pass in a scene and camera but this is just a test for now.
        glRenderer.Render(in SceneCamera);

        GL.Enable(EnableCap.DepthTest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        ImGui.Image(sceneTexture, sceneSize, new(0, 1), new(1, 0));
        ImGui.End();
    }

    void RenderInspector()
    {
        ImGui.Begin("Inspector");
        
        var numLightPos = LightingSettings.LightPos.Numerics();
        if (ImGui.DragFloat3("Camera Position", ref numLightPos))
            LightingSettings.LightPos = numLightPos;

        var numObjectColor = LightingSettings.ObjectColor.Numerics();
        if (ImGui.ColorEdit3("Object Color", ref numObjectColor))
            LightingSettings.ObjectColor = numObjectColor;

        ImGui.DragFloat("Specular Strength", ref LightingSettings.SpecularStrength, 0.01f);
        ImGui.DragFloat("Shininess", ref LightingSettings.Shininess, 1f);

        ImGui.End();
    }

    void RenderDockspace()
    {
        var viewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewport.Pos);
        ImGui.SetNextWindowSize(viewport.Size);
        ImGui.SetNextWindowViewport(viewport.ID);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f);

        var dockFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse |
                        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove |
                        ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus |
                        ImGuiWindowFlags.MenuBar;

        ImGui.Begin("DockSpace", dockFlags);
        var dockspaceID = ImGui.GetID("MainDockSpace");
        ImGui.DockSpace(dockspaceID, new(0, 0), ImGuiDockNodeFlags.PassthruCentralNode);
        ImGui.End();
        ImGui.PopStyleVar(2);
    }

    Vector2 sceneSize = new(0, 0);

    int sceneFBO, sceneTexture, sceneDepth;

    void SetupFramebuffer(int width, int height)
    {
        sceneFBO = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, sceneFBO);
        sceneTexture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, sceneTexture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

        GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear);

        GL.TexParameter(TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear);

        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
            FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D, sceneTexture, 0);

        sceneDepth = GL.GenRenderbuffer();

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, sceneDepth);

        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
            RenderbufferStorage.DepthComponent24, width, height);

        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthAttachment,
            RenderbufferTarget.Renderbuffer, sceneDepth);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            Console.WriteLine("Framebuffer not complete!");

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
}
