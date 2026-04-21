using Chondria.Core;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Windowing;
using ImGuiNET;

namespace Chondria.Editor
{
    [EditorWindow("Scene")]
    internal class SceneWindow : EditorWindow
    {
        Vector2 sceneSize = new(0, 0);

        [EditorWindowDraw]
        public void Draw()
        {
            var sceneSize = ImGui.GetContentRegionAvail();
            this.sceneSize = sceneSize;
            var sceneFocused = ImGui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows);
            if (sceneFocused)
                CameraController.Update(Time.DeltaTime);

            SceneCamera.AspectRatio = sceneSize.X / sceneSize.Y;

            GLRender();

            ImGui.Image(SceneTexture, sceneSize, new(0, 1), new(1, 0));
        }
    }
}
