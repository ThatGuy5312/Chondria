namespace Chondria.Windowing;

[AttributeUsage(AttributeTargets.Method)]
// EditorWindowDraw (how you call it) notes that the method inside of your EditorWindow is the main ImGui draw function.
public class EditorWindowDrawAttribute : Attribute { }