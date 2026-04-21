using Chondria.Core;
using Chondria.Entities;
using Chondria.Management;
using Chondria.Rendering;

namespace Chondria.Windowing;

public class EditorWindow
{
    internal static GLRenderer GLRenderer;

    public static Camera SceneCamera { get; internal set; }

    public static Scene CurrentScene { get; internal set; }

    public static Entity[] SceneEntities { get; internal set; }

    //public static MeshRenderer SelectedEntity;

    internal static CameraController CameraController;

    internal static int SceneTexture;

    internal void GLRender() => MainWindow.Instance.GLRender();

    public virtual bool Active { get; set; } = true;
}
