using Chondria.Core;
using Chondria.Management;
using Chondria.Rendering;

namespace Chondria.Windowing;

internal class MainWindowInfo
{
    public static GLRenderer GLRenderer;

    public static Camera SceneCamera;

    public static CameraController CameraController;

    public static int SceneTexture;

    public void GLRender() => MainWindow.Instance.GLRender();
}
