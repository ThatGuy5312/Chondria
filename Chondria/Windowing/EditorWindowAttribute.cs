namespace Chondria.Windowing;

[AttributeUsage(AttributeTargets.Class)]
// EditorWindow (how you call it), makes the class be found durring reflection for editor windows.
public class EditorWindowAttribute : Attribute
{
    public string Title { get; }

    public EditorWindowAttribute(string title)
    {
        Title = title;
    }
}
