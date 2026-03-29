using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Chondria.Core;

namespace Chondria;

class Program
{
    /*
     
    This is what I hopfully want to be a very long project that I will work on and hopfully be my main focus when I code.

    I know C# and OpenTK arent that ideal of a choice and is probably laughable to some people.
    I do have some experience with C++ and Java but ive used C# ever since I started coding so it just suits me.
     
     */

    // main entrypoint
    static void Main()
    {
        // main window settings
        var nativeSettings = new NativeWindowSettings()
        {
            // look I put a lot of thought into it. yk the phrase "the mitochondria is the powerhouse of the cell" its like "chondria is the powerhouse of your games" 
            Title = "Chondria",
            Flags = ContextFlags.ForwardCompatible,
            WindowState = WindowState.Normal, // Dear ImGui, please let me be able to make a resizable window without throwing a temper tantrum when I add you.
            WindowBorder = WindowBorder.Resizable,
        };

        // run main window
        var window = new MainWindow(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }

    /*
     
     
     3/28/2026 - Made project and played with software rendering.

     3/29/2026 - Added a Input system, mathimatical structs, and a GLRenderer (only renders a cube right now). Also added a camera and camera controller.

     3/30/2026 - Who knows, hasen't happened yet.
     
     
     */
}
