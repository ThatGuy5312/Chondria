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
            Title = "Chondria",
            Flags = ContextFlags.ForwardCompatible,
            WindowState = WindowState.Normal,
            WindowBorder = WindowBorder.Resizable,
        };

        // run main window
        var window = new MainWindow(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }
}
