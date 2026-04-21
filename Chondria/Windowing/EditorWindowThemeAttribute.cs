namespace Chondria.Windowing;

[AttributeUsage(AttributeTargets.Field)]
// EditorWindowTheme (how you call it) is only used it you want to have a custom style you your editor window.
public class EditorWindowThemeAttribute : Attribute { }

/*
 
EXAMPLE USE:

using Chondria.Windowing;
using Chondria.Rendering;

[EditorWindowTheme]
WindowTheme thisWindowTheme = new WindowTheme
{
    WindowBg = Color.Navy,
};

-

Theres a lot more values that you can set and you can also use System.Drawing.Color colors without errors along with Vector3, or Vector4.
Also you dont need to call or reference thisWindowTheme (or whatever you call it) anywhere. The reflection automatically finds it.
And if you dont have it it will just use the normal dark style.
 
 */
