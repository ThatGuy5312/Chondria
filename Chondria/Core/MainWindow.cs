using Chondria.InputSystem;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Management;
using Chondria.Windowing;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Reflection;
using Chondria.Entities;

namespace Chondria.Core;

// main window that displays and runs almost everything
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

    Dictionary<object, (MethodInfo, EditorWindowAttribute, FieldInfo?)> editorWindows = [];

    Entity testObject;

    bool settingsWindowOpen = false;

    public WindowTheme MainWindowTheme = new();

    protected override void OnLoad()
    {
        base.OnLoad();

        //renderer = new Renderer();
        //renderer.Init(Size.X, Size.Y);

        //drawer = new TestPatternDrawer();

        FindEditorWindows();
        ImGuiUtilites.LoadEditorWindows();
        ImGuiUtilites.DeactivateEditorWindow("Settings");

        SetupFramebuffer(Size.X, Size.Y);

        imGuiController = new(this);

        MainWindowTheme.Apply();

        SceneCamera = new(new(0, 0, 3), Size.X / (float)Size.Y);
        CurrentScene = new Scene();
        Scene.LoadScene(CurrentScene);

        cameraController = new(SceneCamera);

        glRenderer = new GLRenderer();
        glRenderer.Init();

        testObject = Entity.Create("Parent", BaseMesh.Cube);
        testObject.Transform.Position = new Vector3(0, 0, 0);
        
        var child1 = Entity.Create("Child 1", BaseMesh.Sphere);
        child1.Transform.Position = new Vector3(2, 0, 0);

        var child2 = Entity.Create("Child 2", BaseMesh.Sphere);
        child2.Transform.Position = new Vector3(-2, 0, 0);

        var light1 = new Entity("Light 1");
        light1.Transform.Position = new Vector3(0, 0, 2);
        var l1 = light1.AddComponent<Light>();
        l1.Color = Color.Indigo;

        var light2 = new Entity("Light 2");
        light2.Transform.Position = new Vector3(0, 0, -2);
        var l2 = light2.AddComponent<Light>();
        l2.Color = Color.Teal;

        testObject.Transform.AddChild(child1.Transform);
        testObject.Transform.AddChild(child2.Transform);

        CurrentScene.AddEntity(light1);
        CurrentScene.AddEntity(light2);

        EditorWindow.GLRenderer = glRenderer;
        EditorWindow.SceneCamera = SceneCamera;
        EditorWindow.CurrentScene = CurrentScene;
        EditorWindow.CameraController = cameraController;
        EditorWindow.SceneEntities = CurrentScene.Objects;//.Select(o => o as MeshRenderer).ToArray();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Time.Update((float)args.Time);

        Input.UpdateInput(MouseState, KeyboardState);

        testObject.Transform.Rotation *= Quaternion.FromEulerAngles(new Vector3(0, Time.DeltaTime * 20f, 0));
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        SetupFramebuffer(e.Width, e.Height);
        EditorWindow.SceneTexture = sceneTexture;
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
            if (!ImGuiUtilites.IsWindowActive(kv.Value.Item2.Title))
                return;

            if (kv.Value.Item3 != null) // i feel like theres a million better ways of doing this but im doing the most obvious worst way
            {
                var theme = (WindowTheme)kv.Value.Item3.GetValue(kv.Key);

                ImGuiUtilites.Begin(kv.Value.Item2.Title, theme);

                kv.Value.Item1.Invoke(kv.Key, null);

                ImGui.End();

                ImGuiUtilites.PopStyleTheme();
            }
            else
            {
                ImGui.Begin(kv.Value.Item2.Title);

                kv.Value.Item1.Invoke(kv.Key, null);

                ImGui.End();
            }
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

            if (ImGui.BeginMenu("Windows"))
            {
                if (ImGui.MenuItem("Settings", "", settingsWindowOpen))
                {
                    settingsWindowOpen = !settingsWindowOpen;
                    ImGuiUtilites.SetEditorWindow("Settings", settingsWindowOpen);
                }
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

                var attribute = type.GetCustomAttribute<EditorWindowAttribute>();

                var method = type.GetMethods()
                    .FirstOrDefault(m => m.GetCustomAttribute<EditorWindowDrawAttribute>() != null);

                var theme = type.GetFields()
                    .FirstOrDefault(f => f.GetCustomAttribute<EditorWindowThemeAttribute>() != null);

               editorWindows.Add(instance, (method, attribute, theme));
            }
        }
    }
}
