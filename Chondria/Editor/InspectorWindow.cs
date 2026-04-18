using Chondria.Entities;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Windowing;
using ImGuiNET;

namespace Chondria.Editor
{
    [EditorWindow("Inspector")]
    internal class InspectorWindow
    {
        [EditorWindowDraw]
        public void Draw()
        {
            if (ImGui.CollapsingHeader("Lighting Settings"))
            {
                for (int i = 0; i < LightingSettings.Lights.Count; i++)
                {
                    var light = LightingSettings.Lights[i];

                    LightingSettings.Lights[i] = DrawLight(light, i);
                }
            }

            if (ImGui.BeginPopupContextWindow("Create Popup", ImGuiPopupFlags.MouseButtonRight))
            {
                ImGui.Text("CREATE");

                ImGui.Separator();

                if (ImGui.MenuItem("Add Light"))
                {
                    LightingSettings.Lights.Add(new Light
                    {
                        Position = new(0, 0, 0),
                        Color = new(1, 1, 1),
                        Linear = 0.09f,
                        Constant = 1f,
                        Quadratic = 0.032f
                    });
                }
                ImGui.EndPopup();
            }

            if (Inspector.NullSelection)
            {
                ImGui.Text("No Entity Selected");
                return;
            }

            DrawEntity(ref Inspector.Selected);
        }

        Light DrawLight(Light light, int index)
        {
            var numLightPos = light.Position.Numerics();
            if (ImGui.DragFloat3($"Light {index} Position", ref numLightPos, 0.1f))
                light.Position = numLightPos.TK();

            var numLightColor = light.Color.Numerics();
            if (ImGui.ColorEdit3($"Light {index} Color", ref numLightColor))
                light.Color = numLightColor.TK();

            ImGui.DragFloat($"Light {index} Linear", ref light.Linear, 0.01f);

            ImGui.DragFloat($"Light {index} Constant", ref light.Constant, 0.1f);

            ImGui.DragFloat($"Light {index} Quadratic", ref light.Quadratic, 0.01f);

            return light;
        }

        void DrawEntity(ref MeshRenderer mr)
        {
            ImGui.Text("Name: " + mr.Name);

            if (ImGui.CollapsingHeader("Transform"))
            {
                var numPos = mr.Transform.Position.Numerics();
                if (ImGui.DragFloat3("Position", ref numPos, 0.1f))
                    mr.Transform.Position = numPos.TK();

                var numRot = mr.Transform.Rotation.ToEulerAngles().Numerics();
                if (ImGui.DragFloat3("Rotation", ref numRot, 0.1f))
                    mr.Transform.Rotation = Quaternion.FromEulerAngles(numRot);

                var numScale = mr.Transform.Scale.Numerics();
                if (ImGui.DragFloat3("Scale", ref numScale, 0.1f))
                    mr.Transform.Scale = numScale.TK();
            }

            if (ImGui.CollapsingHeader("Material"))
            {
                var numColor = mr.Material.Color.Numerics();
                if (ImGui.ColorEdit3("Color", ref numColor))
                    mr.Material.Color = numColor.ChonVec();

                ImGui.DragFloat("Specular Strength", ref mr.Material.SpecularStrength, 0.01f);
                ImGui.DragFloat("Shininess", ref mr.Material.Shininess, 1f);
            }
        }
    }

    public static class Inspector
    {
        public static bool NullSelection => Selected == null;

        public static bool IsSelected(MeshRenderer renderer) => Selected == renderer;

        public static void Select(MeshRenderer renderer) => Selected = renderer;

        public static void Deselect() => Selected = null;

        public static MeshRenderer? Selected;
    }
}
