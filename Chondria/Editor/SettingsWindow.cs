using Chondria.Math;
using Chondria.Rendering;
using Chondria.Windowing;
using ImGuiNET;

namespace Chondria.Editor;

[EditorWindow("Settings")]
internal class SettingsWindow : EditorWindow
{
    WindowTheme theme = new();

    [EditorWindowDraw]
    public void Draw()
    {
        if (ImGui.CollapsingHeader("Color Theme"))
        {
            ColorEdit4("Text", ref theme.Text);
            ColorEdit4("Text Disabled", ref theme.TextDisabled);
            ColorEdit4("Window Background", ref theme.WindowBg);
            ColorEdit4("Child Background", ref theme.ChildBg);
            ColorEdit4("Popup Background", ref theme.PopupBg);
            ColorEdit4("Border", ref theme.Border);
            ColorEdit4("Border Shadow", ref theme.BorderShadow);
            ColorEdit4("Frame Background", ref theme.FrameBg);
            ColorEdit4("Frame Background Hovered", ref theme.FrameBgHovered);
            ColorEdit4("Frame Background Active", ref theme.FrameBgActive);
            ColorEdit4("Title Background", ref theme.TitleBg);
            ColorEdit4("Title Background Active", ref theme.TitleBgActive);
            ColorEdit4("Title Background Collapsed", ref theme.TitleBgCollapsed);
            ColorEdit4("Menu Bar Background", ref theme.MenuBarBg);
            ColorEdit4("Scrollbar Background", ref theme.ScrollbarBg);
            ColorEdit4("Scrollbar Grab", ref theme.ScrollbarGrab);
            ColorEdit4("Scrollbar Grab Hovered", ref theme.ScrollbarGrabHovered);
            ColorEdit4("Scrollbar Grab Active", ref theme.ScrollbarGrabActive);
            ColorEdit4("Check Mark", ref theme.CheckMark);
            ColorEdit4("Slider Grab", ref theme.SliderGrab);
            ColorEdit4("Slider Grab Active", ref theme.SliderGrabActive);
            ColorEdit4("Button", ref theme.Button);
            ColorEdit4("Button Hovered", ref theme.ButtonHovered);
            ColorEdit4("Button Active", ref theme.ButtonActive);
            ColorEdit4("Header", ref theme.Header);
            ColorEdit4("Header Hovered", ref theme.HeaderHovered);
            ColorEdit4("Header Active", ref theme.HeaderActive);
            ColorEdit4("Separator", ref theme.Separator);
            ColorEdit4("Separator Hovered", ref theme.SeparatorHovered);
            ColorEdit4("Separator Active", ref theme.SeparatorActive);
            ColorEdit4("Resize Grip", ref theme.ResizeGrip);
            ColorEdit4("Resize Grip Hovered", ref theme.ResizeGripHovered);
            ColorEdit4("Resize Grip Active", ref theme.ResizeGripActive);
            ColorEdit4("Tab", ref theme.Tab);
            ColorEdit4("Tab Hovered", ref theme.TabHovered);
            ColorEdit4("Tab Selected", ref theme.TabSelected);
            ColorEdit4("Plot Lines", ref theme.PlotLines);
            ColorEdit4("Plot Lines Hovered", ref theme.PlotLinesHovered);
            ColorEdit4("Plot Histogram", ref theme.PlotHistogram);
            ColorEdit4("Plot Histogram Hovered", ref theme.PlotHistogramHovered);
            ColorEdit4("Text Selected Bg", ref theme.TextSelectedBg);
            ColorEdit4("Nav Cursor", ref theme.NavCursor);
            ColorEdit4("Modal Window Dim Bg", ref theme.ModalWindowDimBg);
            DragFloat2("Padding", ref theme.Padding);
            ImGui.DragFloat("Rounding", ref theme.Rounding, 0.1f, 0f, 20f);
        }
        if (ImGui.Button("Apply Theme"))
            theme.Apply();
    }

    void ColorEdit4(string label, ref Color color)
    {
        var c = color.Numerics4();
        if (ImGui.ColorEdit4(label, ref c))
            color = c;
    }
    void DragFloat2(string label, ref Vector2 v)
    {
        var c = v.Numerics();
        if (ImGui.DragFloat2(label, ref c))
            v = c;
    }
}
