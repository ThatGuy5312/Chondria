using Chondria.Core;
using Chondria.Entities;
using Chondria.Management;
using Chondria.Rendering;

namespace Chondria.Windowing;

internal class MainWindowInfo
{
    public static GLRenderer GLRenderer;

    public static Camera SceneCamera;

    public static Scene CurrentScene;

    public static MeshRenderer[] SceneEntities;

    public static MeshRenderer SelectedEntity;

    public static CameraController CameraController;

    public static int SceneTexture;

    public void GLRender() => MainWindow.Instance.GLRender();
}
