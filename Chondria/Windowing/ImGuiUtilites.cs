using Chondria.Math;
using ImGuiNET;
using System.Reflection;

namespace Chondria.Windowing;

// just a helper for making Dear ImGui things without writing a million lines each time you want to do something different.
public static class ImGuiUtilites
{
    static List<EditorWindow> m_EditorWindows = [];

    public static void LoadEditorWindows()
    {
        m_EditorWindows.Clear();

        var assem = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in assem)
        {
            if (type.IsSubclassOf(typeof(EditorWindow)))
            {
                var editorWindow = (Activator.CreateInstance(type) as EditorWindow)!;
                Console.WriteLine($"Found editor window: {type.Name}");
                m_EditorWindows.Add(editorWindow);
            }
        }

    }

    public static void ActivateEditorWindow(string windowName)
    {
        foreach (var editorWindow in m_EditorWindows)
        {
            var type = editorWindow.GetType();

            if (type.GetCustomAttribute<EditorWindowAttribute>() != null)
            {
                var attribute = type.GetCustomAttribute<EditorWindowAttribute>();
                if (attribute.Title == windowName)
                {
                    editorWindow.Active = true;
                    Console.WriteLine($"Activated window {windowName}");
                }
            }
        }
    }

    public static void DeactivateEditorWindow(string windowName)
    {
        foreach (var editorWindow in m_EditorWindows)
        {
            var type = editorWindow.GetType();

            if (type.GetCustomAttribute<EditorWindowAttribute>() != null)
            {
                var attribute = type.GetCustomAttribute<EditorWindowAttribute>();
                if (attribute.Title == windowName)
                {
                    editorWindow.Active = false;
                    Console.WriteLine($"Deactivated window {windowName}");
                }
            }
        }
    }

    public static void SetEditorWindow(string windowName, bool value)
    {
        foreach (var editorWindow in m_EditorWindows)
        {
            var type = editorWindow.GetType();

            if (type.GetCustomAttribute<EditorWindowAttribute>() != null)
            {
                var attribute = type.GetCustomAttribute<EditorWindowAttribute>();
                if (attribute.Title == windowName)
                    editorWindow.Active = value;
            }
        }
    }

    public static bool IsWindowActive(string windowName)
    {
        foreach (var editorWindow in m_EditorWindows)
        {
            var type = editorWindow.GetType();

            if (type.GetCustomAttribute<EditorWindowAttribute>() != null)
            {
                var attribute = type.GetCustomAttribute<EditorWindowAttribute>();
                if (attribute.Title == windowName)
                    return editorWindow.Active;
            }
        }
        return false;
    }

    public static void Begin(string title, WindowTheme wintheme)
    {
        PushStyleColor(wintheme);

        ImGui.Begin(title);
    }

    public static void Begin(string title, ImGuiWindowFlags flags, WindowTheme wintheme)
    {
        PushStyleColor(wintheme);

        ImGui.Begin(title, flags);
    }

    public static void PushStyleColor(WindowTheme theme)
    {
        /*
         Dear ImGui, please make it so you can push a RangeAccesor<System.Numerics.Vector4> using PushStyleColor.
         */

        ImGui.PushStyleColor(ImGuiCol.Text, theme.Text);
        ImGui.PushStyleColor(ImGuiCol.TextDisabled, theme.TextDisabled);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, theme.WindowBg);

        ImGui.PushStyleColor(ImGuiCol.ChildBg, theme.ChildBg);
        ImGui.PushStyleColor(ImGuiCol.PopupBg, theme.PopupBg);
        ImGui.PushStyleColor(ImGuiCol.Border, theme.Border);

        ImGui.PushStyleColor(ImGuiCol.FrameBg, theme.FrameBg);
        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, theme.FrameBgHovered);
        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, theme.FrameBgActive);

        ImGui.PushStyleColor(ImGuiCol.TitleBg, theme.TitleBg);
        ImGui.PushStyleColor(ImGuiCol.TitleBgActive, theme.TitleBgActive);
        ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, theme.TitleBgCollapsed);

        ImGui.PushStyleColor(ImGuiCol.MenuBarBg, theme.MenuBarBg);

        ImGui.PushStyleColor(ImGuiCol.ScrollbarBg, theme.ScrollbarBg);
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrab, theme.ScrollbarGrab);
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabHovered, theme.ScrollbarGrabHovered);
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabActive, theme.ScrollbarGrabActive);

        ImGui.PushStyleColor(ImGuiCol.CheckMark, theme.CheckMark);

        ImGui.PushStyleColor(ImGuiCol.SliderGrab, theme.SliderGrab);
        ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, theme.SliderGrabActive);

        ImGui.PushStyleColor(ImGuiCol.Button, theme.Button);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, theme.ButtonHovered);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, theme.ButtonActive);

        ImGui.PushStyleColor(ImGuiCol.Header, theme.Header);
        ImGui.PushStyleColor(ImGuiCol.HeaderHovered, theme.HeaderHovered);
        ImGui.PushStyleColor(ImGuiCol.HeaderActive, theme.HeaderActive);

        ImGui.PushStyleColor(ImGuiCol.Separator, theme.Separator);
        ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, theme.SeparatorHovered);
        ImGui.PushStyleColor(ImGuiCol.SeparatorActive, theme.SeparatorActive);

        ImGui.PushStyleColor(ImGuiCol.ResizeGrip, theme.ResizeGrip);
        ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, theme.ResizeGripHovered);
        ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, theme.ResizeGripActive);

        ImGui.PushStyleColor(ImGuiCol.Tab, theme.Tab);
        ImGui.PushStyleColor(ImGuiCol.TabHovered, theme.TabHovered);
        ImGui.PushStyleColor(ImGuiCol.TabActive, theme.TabSelected);

        ImGui.PushStyleColor(ImGuiCol.PlotLines, theme.PlotLines);
        ImGui.PushStyleColor(ImGuiCol.PlotLinesHovered, theme.PlotLinesHovered);
        ImGui.PushStyleColor(ImGuiCol.PlotHistogram, theme.PlotHistogram);
        ImGui.PushStyleColor(ImGuiCol.PlotHistogramHovered, theme.PlotHistogramHovered);
        ImGui.PushStyleColor(ImGuiCol.TextSelectedBg, theme.TextSelectedBg);

        ImGui.PushStyleColor(ImGuiCol.NavHighlight, theme.NavCursor);
        ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, theme.ModalWindowDimBg);
    }

    public static void PopStyleTheme()
    {
        ImGui.PopStyleColor(42);
    }

    public static void BubbleText(string text, Vector2 padding)
    {
        var drawList = ImGui.GetWindowDrawList();
        var pos = ImGui.GetCursorScreenPos();

        var size = ImGui.CalcTextSize(text);

        drawList.AddRectFilled(
            pos - padding.Numerics(),
            pos + size + padding.Numerics(),
            ImGui.GetColorU32(new Vector4(0.15f, 0.15f, 0.15f, 0.9f)),
            8f
        );

        drawList.AddText(pos, ImGui.GetColorU32(Vector4.One), text);

        ImGui.Dummy(size + padding.Numerics() * 2);
    }
}
