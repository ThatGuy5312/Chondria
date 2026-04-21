using Chondria.Entities;
using Chondria.Windowing;
using ImGuiNET;
using Chondria.Math;

namespace Chondria.Editor;

[EditorWindow("Hierarchy")]
internal class HierarchWindow : EditorWindow
{
    [EditorWindowDraw]
    public void Draw()
    {
        if (ImGui.BeginPopupContextWindow("entity_creation_popup", ImGuiPopupFlags.MouseButtonRight))
        {
            ImGui.TextDisabled("CREATE");

            ImGui.Separator();



            ImGui.EndPopup();
        }

        var sceneText = CurrentScene == null ? "No Scene Selected" : CurrentScene.Name;
        ImGuiUtilites.BubbleText(sceneText, new(5));

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

    static void DrawNode(Entity obj)
    {
        bool hasChildren = obj.Transform.Children.Count > 0;

        if (!hasChildren)
        {
            if (ImGui.Selectable(obj.Name, Inspector.IsSelected(obj)))
            {
                Inspector.Select(obj);
            }
            return;
        }

        var flags =
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
