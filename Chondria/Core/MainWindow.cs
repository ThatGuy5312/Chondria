using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Management;
using Chondria.Windowing;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Reflection;
using Chondria.Entities;

namespace Chondria.Core;

// main window for everything
internal class MainWindow : GameWindow
{
    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) => 
        Instance = this;

    public static MainWindow Instance { get; private set; }

    // old things
    //private Renderer renderer;
    // temperary but kinda like a render pipeline
    //private IDrawer drawer;

    public ImGuiController imGuiController;

    public GLRenderer glRenderer;
    public Camera SceneCamera;

    public CameraController cameraController;

    public Scene CurrentScene;

    Dictionary<object, MethodInfo> editorWindows = [];

    protected override void OnLoad()
    {
        base.OnLoad();

        //renderer = new Renderer();
        //renderer.Init(Size.X, Size.Y);

        //drawer = new TestPatternDrawer();

        FindEditorWindows();

        SetupFramebuffer(Size.X, Size.Y);

        imGuiController = new(this);

        SceneCamera = new(new(0, 0, 3), Size.X / (float)Size.Y);
        CurrentScene = new Scene();

        cameraController = new(SceneCamera);

        glRenderer = new GLRenderer();
        glRenderer.Init();

        float[] vertices = {
            // position        // normal
            -0.5f, -0.5f, 0.5f,  0, 0, 1,
             0.5f, -0.5f, 0.5f,  0, 0, 1,
             0.5f,  0.5f, 0.5f,  0, 0, 1,

            0.5f,  0.5f, 0.5f,  0, 0, 1,
            -0.5f,  0.5f, 0.5f,  0, 0, 1,
            -0.5f, -0.5f, 0.5f,  0, 0, 1,

            -0.5f, -0.5f, -0.5f,  0, 0, -1,
            0.5f, -0.5f, -0.5f,  0, 0, -1,
            0.5f, 0.5f, -0.5f,  0, 0, -1,

            0.5f, 0.5f, -0.5f,  0, 0, -1,
            -0.5f, 0.5f, -0.5f,  0, 0, -1,
            -0.5f, -0.5f, -0.5f,  0, 0, -1,

            -0.5f, 0.5f, 0.5f,  -1, 0, 0,
            -0.5f, 0.5f, -0.5f,  -1, 0, 0,
            -0.5f, -0.5f, -0.5f,  -1, 0, 0,

            -0.5f, -0.5f, -0.5f,  -1, 0, 0,
            -0.5f, -0.5f, 0.5f,  -1, 0, 0,
            -0.5f, 0.5f, 0.5f,  -1, 0, 0,

            0.5f, 0.5f, 0.5f,  1, 0, 0,
            0.5f, 0.5f, -0.5f,  1, 0, 0,
            0.5f, -0.5f, -0.5f,  1, 0, 0,

            0.5f, -0.5f, -0.5f,  1, 0, 0,
            0.5f, -0.5f, 0.5f,  1, 0, 0,
            0.5f, 0.5f, 0.5f,  1, 0, 0,

            -0.5f, 0.5f, -0.5f,  0, 1, 0,
            0.5f, 0.5f, -0.5f,  0, 1, 0,
            0.5f, 0.5f, 0.5f,  0, 1, 0,

            0.5f, 0.5f, 0.5f,  0, 1, 0,
            -0.5f, 0.5f, 0.5f,  0, 1, 0,
            -0.5f, 0.5f, -0.5f,  0, 1, 0,

            -0.5f, -0.5f, -0.5f,  0, -1, 0,
            0.5f, -0.5f, -0.5f,  0, -1, 0,
            0.5f, -0.5f, 0.5f,  0, -1, 0,

            0.5f, -0.5f, 0.5f,  0, -1, 0,
            -0.5f, -0.5f, 0.5f,  0, -1, 0,
            -0.5f, -0.5f, -0.5f,  0, -1, 0,
        };

        var mesh = new Mesh(vertices);

        var meshRenderer = new MeshRenderer(mesh);

        meshRenderer.Material.Color = new Vector3(0, 1, 0);

        var mesh2 = new Mesh(vertices);

        var meshRenderer2 = new MeshRenderer(mesh);

        meshRenderer2.Material.Color = new Vector3(1, 0, 0);

        meshRenderer2.Transform.Position = new Vector3(2, 0, 0);

        CurrentScene.Add(meshRenderer);
        CurrentScene.Add(meshRenderer2);

        MainWindowInfo.GLRenderer = glRenderer;
        MainWindowInfo.SceneCamera = SceneCamera;
        MainWindowInfo.CurrentScene = CurrentScene;
        MainWindowInfo.CameraController = cameraController;
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
        MainWindowInfo.SceneTexture = sceneTexture;
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

        RenderMenuBar();

        //glRenderer.Render(in SceneCamera);
        //RenderScene();

        editorWindows.ToList().ForEach(kv =>
        {
            ImGui.Begin(kv.Key.GetType().Name);

            kv.Value.Invoke(kv.Key, null);

            ImGui.End();
        });

        imGuiController.Render();

        GL.Disable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);

        SwapBuffers();
    }

    public void GLRender()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, sceneFBO);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Enable(EnableCap.DepthTest);

        // renders the the cube while passing in a camera.
        // I plan to eventually pass in a scene and camera but this is just a test for now.
        glRenderer.Render(in CurrentScene, in SceneCamera);

        GL.Enable(EnableCap.DepthTest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
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

    void RenderMenuBar()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Exit"))
                    Close();
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }
    }

    public Vector2 sceneSize = new(0, 0);

    public int sceneFBO, sceneTexture, sceneDepth;

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

    void FindEditorWindows()
    {
        editorWindows.Clear();

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.GetCustomAttribute<EditorWindowAttribute>() != null)
            {
                var instance = Activator.CreateInstance(type);

                var method = type.GetMethods()
                    .FirstOrDefault(m => m.GetCustomAttribute<EditorWindowDrawAttribute>() != null);

               editorWindows.Add(instance, method);
            }
        }
    }
}
