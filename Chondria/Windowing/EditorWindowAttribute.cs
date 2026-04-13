namespace Chondria.Windowing;

[AttributeUsage(AttributeTargets.Class)]
internal class EditorWindowAttribute : Attribute
{
    public string Title { get; }

    public EditorWindowAttribute(string title)
    {
        Title = title;
    }
}
