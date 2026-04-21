using Chondria.Math;
using Chondria.Rendering;
using ImGuiNET;

namespace Chondria.Windowing;

public struct WindowTheme()
{
    public Color Text = new(1.00f, 1.00f, 1.00f, 1.00f);
    public Color TextDisabled = new(0.50f, 0.50f, 0.50f, 1.00f);

    public Color WindowBg = new(0.06f, 0.06f, 0.06f, 0.94f);
    public Color ChildBg = new(0.00f, 0.00f, 0.00f, 0.00f);
    public Color PopupBg = new(0.08f, 0.08f, 0.08f, 0.94f);

    public Color Border = new(0.43f, 0.43f, 0.50f, 0.50f);
    public Color BorderShadow = new(0.00f, 0.00f, 0.00f, 0.00f);

    public Color FrameBg = new(0.16f, 0.29f, 0.48f, 0.54f);
    public Color FrameBgHovered = new(0.26f, 0.59f, 0.98f, 0.40f);
    public Color FrameBgActive = new(0.26f, 0.59f, 0.98f, 0.67f);

    public Color TitleBg = new(0.04f, 0.04f, 0.04f, 1.00f);
    public Color TitleBgActive = new(0.16f, 0.29f, 0.48f, 1.00f);
    public Color TitleBgCollapsed = new(0.00f, 0.00f, 0.00f, 0.51f);

    public Color MenuBarBg = new(0.14f, 0.14f, 0.14f, 1.00f);

    public Color ScrollbarBg = new(0.02f, 0.02f, 0.02f, 0.53f);
    public Color ScrollbarGrab = new(0.31f, 0.31f, 0.31f, 1.00f);
    public Color ScrollbarGrabHovered = new(0.41f, 0.41f, 0.41f, 1.00f);
    public Color ScrollbarGrabActive = new(0.51f, 0.51f, 0.51f, 1.00f);

    public Color CheckMark = new(0.26f, 0.59f, 0.98f, 1.00f);

    public Color SliderGrab = new(0.24f, 0.52f, 0.88f, 1.00f);
    public Color SliderGrabActive = new(0.26f, 0.59f, 0.98f, 1.00f);

    public Color Button = new(0.26f, 0.59f, 0.98f, 0.40f);
    public Color ButtonHovered = new(0.26f, 0.59f, 0.98f, 1.00f);
    public Color ButtonActive = new(0.06f, 0.53f, 0.98f, 1.00f);

    public Color Header = new(0.26f, 0.59f, 0.98f, 0.31f);
    public Color HeaderHovered = new(0.26f, 0.59f, 0.98f, 0.80f);
    public Color HeaderActive = new(0.26f, 0.59f, 0.98f, 1.00f);

    public Color Separator = new(0.43f, 0.43f, 0.50f, 0.50f);
    public Color SeparatorHovered = new(0.10f, 0.40f, 0.75f, 0.78f);
    public Color SeparatorActive = new(0.10f, 0.40f, 0.75f, 1.00f);

    public Color ResizeGrip = new(0.26f, 0.59f, 0.98f, 0.20f);
    public Color ResizeGripHovered = new(0.26f, 0.59f, 0.98f, 0.67f);
    public Color ResizeGripActive = new(0.26f, 0.59f, 0.98f, 0.95f);

    public Color TabHovered = new(0.26f, 0.59f, 0.98f, 0.80f);
    public Color Tab = new(0.18f, 0.35f, 0.58f, 0.86f);
    public Color TabSelected = new(0.20f, 0.41f, 0.68f, 1.00f);

    public Color PlotLines = new(0.61f, 0.61f, 0.61f, 1.00f);
    public Color PlotLinesHovered = new(1.00f, 0.43f, 0.35f, 1.00f);
    public Color PlotHistogram = new(0.90f, 0.70f, 0.00f, 1.00f);
    public Color PlotHistogramHovered = new(1.00f, 0.60f, 0.00f, 1.00f);

    public Color TextSelectedBg = new(0.26f, 0.59f, 0.98f, 0.35f);

    public Color NavCursor = new(0.26f, 0.59f, 0.98f, 1.00f);
    public Color ModalWindowDimBg = new(0.80f, 0.80f, 0.80f, 0.35f);

    public Vector2 Padding = new(8, 8);

    public float Rounding = 0f;

    public void Apply()
    {
        var colors = ImGui.GetStyle().Colors;

        colors[(int)ImGuiCol.Text] = Text;
        colors[(int)ImGuiCol.TextDisabled] = TextDisabled;
        colors[(int)ImGuiCol.WindowBg] = WindowBg;
        colors[(int)ImGuiCol.ChildBg] = ChildBg;
        colors[(int)ImGuiCol.PopupBg] = PopupBg;
        colors[(int)ImGuiCol.Border] = Border;
        colors[(int)ImGuiCol.BorderShadow] = BorderShadow;
        colors[(int)ImGuiCol.FrameBg] = FrameBg;
        colors[(int)ImGuiCol.FrameBgHovered] = FrameBgHovered;
        colors[(int)ImGuiCol.FrameBgActive] = FrameBgActive;
        colors[(int)ImGuiCol.TitleBg] = TitleBg;
        colors[(int)ImGuiCol.TitleBgActive] = TitleBgActive;
        colors[(int)ImGuiCol.TitleBgCollapsed] = TitleBgCollapsed;
        colors[(int)ImGuiCol.MenuBarBg] = MenuBarBg;
        colors[(int)ImGuiCol.ScrollbarBg] = ScrollbarBg;
        colors[(int)ImGuiCol.ScrollbarGrab] = ScrollbarGrab;
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = ScrollbarGrabHovered;
        colors[(int)ImGuiCol.ScrollbarGrabActive] = ScrollbarGrabActive;
        colors[(int)ImGuiCol.CheckMark] = CheckMark;
        colors[(int)ImGuiCol.SliderGrab] = SliderGrab;
        colors[(int)ImGuiCol.SliderGrabActive] = SliderGrabActive;
        colors[(int)ImGuiCol.Button] = Button;
        colors[(int)ImGuiCol.ButtonHovered] = ButtonHovered;
        colors[(int)ImGuiCol.ButtonActive] = ButtonActive;
        colors[(int)ImGuiCol.Header] = Header;
        colors[(int)ImGuiCol.HeaderHovered] = HeaderHovered;
        colors[(int)ImGuiCol.HeaderActive] = HeaderActive;
        colors[(int)ImGuiCol.Separator] = Separator;
        colors[(int)ImGuiCol.SeparatorHovered] = SeparatorHovered;
        colors[(int)ImGuiCol.SeparatorActive] = SeparatorActive;
        colors[(int)ImGuiCol.ResizeGrip] = ResizeGrip;
        colors[(int)ImGuiCol.ResizeGripHovered] = ResizeGripHovered;
        colors[(int)ImGuiCol.ResizeGripActive] = ResizeGripActive;
        colors[(int)ImGuiCol.Tab] = Tab;
        colors[(int)ImGuiCol.TabHovered] = TabHovered;
        colors[(int)ImGuiCol.TabActive] = TabSelected;
        colors[(int)ImGuiCol.TabUnfocused] = Tab;
        colors[(int)ImGuiCol.TabUnfocusedActive] = TabSelected;
        colors[(int)ImGuiCol.PlotLines] = PlotLines;
        colors[(int)ImGuiCol.PlotLinesHovered] = PlotLinesHovered;
        colors[(int)ImGuiCol.PlotHistogram] = PlotHistogram;
        colors[(int)ImGuiCol.PlotHistogramHovered] = PlotHistogramHovered;
        colors[(int)ImGuiCol.TextSelectedBg] = TextSelectedBg;
        colors[(int)ImGuiCol.DragDropTarget] = NavCursor;
        colors[(int)ImGuiCol.NavHighlight] = NavCursor;
        colors[(int)ImGuiCol.NavWindowingHighlight] = NavCursor;
        colors[(int)ImGuiCol.ModalWindowDimBg] = ModalWindowDimBg;
        var style = ImGui.GetStyle();
        style.WindowPadding = Padding;
        style.FrameRounding = Rounding;
    }
}
