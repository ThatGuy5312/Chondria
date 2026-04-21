using Chondria.Math;
using Chondria.Rendering;
using Chondria.Windowing;
using ImGuiNET;

namespace Chondria.Entities;

public class Light : Component
{
    public Light() { Name = "Light"; }

    public Color Color;

    public float Constant = 1f;
    public float Linear = 0.09f;
    public float Quadratic = 0.032f;

    internal override void OnEditorGui()
    {
        var color = Color.Numerics();
        if (ImGui.ColorEdit3("Color", ref color))
            Color = color;

        ImGui.DragFloat("Constant", ref Constant);
        ImGui.DragFloat("Linear", ref Linear);
        ImGui.DragFloat("Quadratic", ref Quadratic);
    }
}
