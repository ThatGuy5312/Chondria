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
            WindowState = WindowState.Maximized, // Dear ImGui, please let me be able to make a resizable window without throwing a temper tantrum when I add you.
            WindowBorder = WindowBorder.Resizable,
        };

        // run main window
        var window = new MainWindow(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }

    /*
     
     
     3/28/2026 - Made project and played with software rendering.

     3/29/2026 - Added a Input system, mathimatical structs, and a GLRenderer (only renders a cube right now). Also added a camera and camera controller.

     i forgot what day it is. its 3/30 right now but its also 2 in the morning. and i worked on the last update yesterday. but i didn't work on anything today.
     actually i did do something and that was add a interger version of the vector struct. but i dont know it i did that today or yesterday or tomorrow. 
     whatever im to tired for this and i dont feel like updating the github just for some vector additions.

     3/30/2026 - Who knows, hasen't happened yet.

     was on spring break and went to arizona for a week

     4/9/2026 - Added a Quaternion struct, added a Key enum for the input system, and made the camera better along with its freecam and also made the viewport resizable.
     
     4/10/2026 - Added ImGui (still finicky) and played with shaders. Probably not gonna push this to github today.

     4/11/2026 - Did some black magic with ImGui to fix the mouse offset and to make it work well while being windowed and made the shader be adjustable during runtime. Also added a Time class.

     4/12/2026 - Added a orbital camera mode, added mutiple light support to the shader, and made a window system. Also added a Material class and change the Scene folder name to Management. Oh yeah and I also added a Color struct.
     
     */
}
