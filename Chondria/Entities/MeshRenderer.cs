using Chondria.Math;
using Chondria.Rendering;
using ImGuiNET;

namespace Chondria.Entities
{
    public class MeshRenderer : Component
    {
        public Material Material = new();
        public Mesh? Mesh { get; set; }

        public MeshRenderer()
        {
            Name = "MeshRenderer";
        }

        internal void Render(Shader shader)
        {
            shader.SetMatrix4("model", Transform.Matrix);
        }

        internal override void OnEditorGui()
        {
            var color = Material.Color.Numerics();
            if (ImGui.ColorEdit3("Color", ref color))
                Material.Color = color;

            ImGui.DragFloat("Specular Strength", ref Material.SpecularStrength);
            ImGui.DragFloat("Shininess", ref Material.Shininess);
        }
    }
}
