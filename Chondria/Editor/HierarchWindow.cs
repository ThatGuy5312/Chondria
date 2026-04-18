using Chondria.Entities;
using Chondria.Windowing;
using ImGuiNET;
using System.Numerics;

namespace Chondria.Editor;

[EditorWindow("Hierarchy")]
internal class HierarchWindow : MainWindowInfo
{
    [EditorWindowDraw]
    public void Draw()
    {
        var sceneText = CurrentScene == null ? "No Scene Selected" : CurrentScene.Name;
        BubbleText(sceneText, new(5));

        ImGui.Separator();

        if (CurrentScene == null)
            return;

        for (int i = 0; i < CurrentScene.Objects.Length; i++)
        {
            var obj = CurrentScene.Objects[i];

            if (obj.Transform.Parent == null)
            {
                DrawNode(obj);
            }

            //if (ImGui.Selectable(obj.Name, SelectedEntity == obj))
            //    SelectedEntity = obj;
        }
    }

    // might make a UI Utils class later to put stuff like this in
    public static void BubbleText(string text, Vector2 padding)
    {
        var drawList = ImGui.GetWindowDrawList();
        var pos = ImGui.GetCursorScreenPos();

        var size = ImGui.CalcTextSize(text);

        drawList.AddRectFilled(
            pos - padding,
            pos + size + padding,
            ImGui.GetColorU32(new Vector4(0.15f, 0.15f, 0.15f, 0.9f)),
            8f
        );

        drawList.AddText(pos, ImGui.GetColorU32(Vector4.One), text);

        ImGui.Dummy(size + padding * 2);
    }

    static void DrawNode(MeshRenderer obj)
    {
        bool hasChildren = obj.Transform.Children.Count > 0;

        ImGuiTreeNodeFlags flags =
            ImGuiTreeNodeFlags.OpenOnArrow |
            ImGuiTreeNodeFlags.SpanAvailWidth;

        if (Inspector.IsSelected(obj))
            flags |= ImGuiTreeNodeFlags.Selected;

        bool opened = ImGui.TreeNodeEx(obj.Name, flags);

        // Click selection
        if (ImGui.IsItemClicked())
        {
            Inspector.Select(obj);
        }

        if (opened)
        {
            foreach (var child in obj.Transform.Children)
            {
                if (child.Children.Count != 0)
                {
                    DrawNode(child.Entity);
                }
                else
                {
                    if (ImGui.Selectable(child.Entity.Name, Inspector.IsSelected(child.Entity)))
                    {
                        Inspector.Select(child.Entity);
                    }
                }
            }

            ImGui.TreePop();
        }
    }
}
