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

                    var numLightPos = light.Position.Numerics();
                    if (ImGui.DragFloat3($"Light {i} Position", ref numLightPos, 0.1f))
                        light.Position = numLightPos.TK();

                    var numLightColor = light.Color.Numerics();
                    if (ImGui.ColorEdit3($"Light {i} Color", ref numLightColor))
                        light.Color = numLightColor.TK();

                    ImGui.DragFloat($"Light {i} Linear", ref light.Linear, 0.01f);

                    ImGui.DragFloat($"Light {i} Constant", ref light.Constant, 0.1f);

                    ImGui.DragFloat($"Light {i} Quadratic", ref light.Quadratic, 0.01f);

                    LightingSettings.Lights[i] = light;
                }
            }

            if (ImGui.BeginPopupContextWindow())
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

            ImGui.Separator();

            var numObjectColor = LightingSettings.Material.Color.Numerics();
            if (ImGui.ColorEdit3("Object Color", ref numObjectColor))
                LightingSettings.Material.Color = numObjectColor.ChonVec();

            ImGui.DragFloat("Specular Strength", ref LightingSettings.Material.SpecularStrength, 0.01f);
            ImGui.DragFloat("Shininess", ref LightingSettings.Material.Shininess, 1f);
        }
    }
}
