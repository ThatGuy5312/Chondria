using Chondria.Math;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Chondria.InputSystem
{
    // Made this a while back for my first game engine attempt that will never and I MEAN never see the light of day again. That code was atrocious.
    public static class Input
    {
        static readonly BaseInputModule module;

        static Input() => module = new();

        public static Vector2 MouseDelta { get; private set; }
        public static Vector2 ScrollDelta { get; private set; }
        public static Vector2 Scroll { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        static Keys KeyToKeys(Key key)
        {
            if (Enum.TryParse<Keys>(key.ToString(), true, out var parsedKey))
                return parsedKey;

            return Keys.Unknown;
        }

        public static void UpdateInput(MouseState mouse, KeyboardState keyboard)
        {
            module.Mouse = mouse;
            module.Keyboard = keyboard;

            foreach (var btn in Enum.GetValues<MouseButton>())
            {
                module.mouseStates[btn] = mouse.IsButtonDown(btn);

                module.mousePressedStates[btn] = mouse.IsButtonPressed(btn);

                module.mouseReleasedStates[btn] = mouse.IsButtonReleased(btn);
            }

            module.mouseDelta = mouse.Delta;
            MouseDelta = module.mouseDelta;

            module.mouseScrollDelta = mouse.ScrollDelta;
            ScrollDelta = module.mouseScrollDelta;

            module.mouseScroll = mouse.Scroll;
            Scroll = module.mouseScroll;

            module.mousePosition = mouse.Position;
            MousePosition = module.mousePosition;

            foreach (var key in Enum.GetValues<Key>())
            {
                var keys = KeyToKeys(key);

                if (keys == Keys.Unknown)
                    continue;

                module.keyStates[key] = keyboard.IsKeyDown(keys);

                module.keyPressedStates[key] = keyboard.IsKeyPressed(keys);

                module.keyReleasedStates[key] = keyboard.IsKeyReleased(keys);
            }
        }

        /// <summary>Gets the mouse button that is down.</summary>
        /// <param name="button">The button index</param>
        /// <returns>True if the mouse button is down and false otherwise</returns>
        public static bool IsMouseDown(int button) => module.mouseStates[(MouseButton)button];

        /// <summary>Gets the mouse button that was pressed.</summary>
        /// <param name="button">The button index</param>
        /// <returns>True if the mouse button was pressed and false otherwise</returns>
        public static bool WasMousePressed(int button) => module.mousePressedStates[(MouseButton)button];

        /// <summary>Gets the mouse button that was released.</summary>
        /// <param name="button">The button index</param>
        /// <returns>True if the mouse button was released and false otherwise</returns>
        public static bool WasMouseReleased(int button) => module.mouseReleasedStates[(MouseButton)button];

        /// <summary>Gets the keys that is down.</summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key is down and false otherwise</returns>
        public static bool IsKeyDown(Key key) => module.keyStates[key];

        /// <summary>Gets the keys that was just pressed.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just pressed and false otherwise</returns>
        public static bool WasKeyPressed(Key key) => module.keyPressedStates[key];

        /// <summary>Gets the keys that was just released.</summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key was just released and false otherwise</returns>
        public static bool WasKeyReleased(Key key) => module.keyReleasedStates[key];

        /// <summary>Gets the keys that is down.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key is down and false otherwise</returns>
        public static bool IsKeyDown(string key)
        {
            if (Enum.TryParse<Key>(key, true, out var parsedKey))
                return module.keyStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets the keys that was just pressed.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just pressed and false otherwise</returns>
        public static bool WasKeyPressed(string key)
        {
            if (Enum.TryParse<Key>(key, true, out var parsedKey))
                return module.keyPressedStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets the keys that was just released.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just released and false otherwise</returns>
        public static bool WasKeyReleased(string key)
        {
            if (Enum.TryParse<Key>(key, true, out var parsedKey))
                return module.keyReleasedStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets all the current keys that are being pressed.</summary>
        /// <returns>Keys[] a list of the pressed keys.</returns>
        public static Key[] AllKeysDown()
        {
            List<Key> result = [];
            foreach (var states in module.keyStates)
            {
                if (states.Value)
                    result.Add(states.Key);
                else result.Remove(states.Key);
            }
            return [.. result];
        }

        /// <summary>Gets the value for the axis you input.</summary>
        /// <param name="axis">Types: 'Horizontal', 'Vertical', 'MouseX', 'MouseY'</param>
        /// <returns>A float value for basic wasd and mouse input.</returns>
        public static float GetAxis(string axis) => axis switch
        {
            "Horizontal" => (IsKeyDown(Key.A) ? 1f : 0f) - (IsKeyDown(Key.D) ? 1f : 0f),
            "Vertical" => (IsKeyDown(Key.W) ? 1f : 0f) - (IsKeyDown(Key.S) ? 1f : 0f),
            "MouseX" => MousePosition.X,
            "MouseY" => MousePosition.Y,
            _ => 0f
        };
    }

    // i just copied the code from the openTK GraphicsLibraryFramework.Keys
    public enum Key
    {
        //
        // Summary:
        //     An unknown key.
        Unknown = -1,
        //
        // Summary:
        //     The spacebar key.
        Space = 32,
        //
        // Summary:
        //     The apostrophe key.
        Apostrophe = 39,
        //
        // Summary:
        //     The comma key.
        Comma = 44,
        //
        // Summary:
        //     The minus key.
        Minus = 45,
        //
        // Summary:
        //     The period key.
        Period = 46,
        //
        // Summary:
        //     The slash key.
        Slash = 47,
        //
        // Summary:
        //     The 0 key.
        D0 = 48,
        //
        // Summary:
        //     The 1 key.
        D1 = 49,
        //
        // Summary:
        //     The 2 key.
        D2 = 50,
        //
        // Summary:
        //     The 3 key.
        D3 = 51,
        //
        // Summary:
        //     The 4 key.
        D4 = 52,
        //
        // Summary:
        //     The 5 key.
        D5 = 53,
        //
        // Summary:
        //     The 6 key.
        D6 = 54,
        //
        // Summary:
        //     The 7 key.
        D7 = 55,
        //
        // Summary:
        //     The 8 key.
        D8 = 56,
        //
        // Summary:
        //     The 9 key.
        D9 = 57,
        //
        // Summary:
        //     The semicolon key.
        Semicolon = 59,
        //
        // Summary:
        //     The equal key.
        Equal = 61,
        //
        // Summary:
        //     The A key.
        A = 65,
        //
        // Summary:
        //     The B key.
        B = 66,
        //
        // Summary:
        //     The C key.
        C = 67,
        //
        // Summary:
        //     The D key.
        D = 68,
        //
        // Summary:
        //     The E key.
        E = 69,
        //
        // Summary:
        //     The F key.
        F = 70,
        //
        // Summary:
        //     The G key.
        G = 71,
        //
        // Summary:
        //     The H key.
        H = 72,
        //
        // Summary:
        //     The I key.
        I = 73,
        //
        // Summary:
        //     The J key.
        J = 74,
        //
        // Summary:
        //     The K key.
        K = 75,
        //
        // Summary:
        //     The L key.
        L = 76,
        //
        // Summary:
        //     The M key.
        M = 77,
        //
        // Summary:
        //     The N key.
        N = 78,
        //
        // Summary:
        //     The O key.
        O = 79,
        //
        // Summary:
        //     The P key.
        P = 80,
        //
        // Summary:
        //     The Q key.
        Q = 81,
        //
        // Summary:
        //     The R key.
        R = 82,
        //
        // Summary:
        //     The S key.
        S = 83,
        //
        // Summary:
        //     The T key.
        T = 84,
        //
        // Summary:
        //     The U key.
        U = 85,
        //
        // Summary:
        //     The V key.
        V = 86,
        //
        // Summary:
        //     The W key.
        W = 87,
        //
        // Summary:
        //     The X key.
        X = 88,
        //
        // Summary:
        //     The Y key.
        Y = 89,
        //
        // Summary:
        //     The Z key.
        Z = 90,
        //
        // Summary:
        //     The left bracket(opening bracket) key.
        LeftBracket = 91,
        //
        // Summary:
        //     The backslash.
        Backslash = 92,
        //
        // Summary:
        //     The right bracket(closing bracket) key.
        RightBracket = 93,
        //
        // Summary:
        //     The grave accent key.
        GraveAccent = 96,
        //
        // Summary:
        //     The escape key.
        Escape = 256,
        //
        // Summary:
        //     The enter key.
        Enter = 257,
        //
        // Summary:
        //     The tab key.
        Tab = 258,
        //
        // Summary:
        //     The backspace key.
        Backspace = 259,
        //
        // Summary:
        //     The insert key.
        Insert = 260,
        //
        // Summary:
        //     The delete key.
        Delete = 261,
        //
        // Summary:
        //     The right arrow key.
        Right = 262,
        //
        // Summary:
        //     The left arrow key.
        Left = 263,
        //
        // Summary:
        //     The down arrow key.
        Down = 264,
        //
        // Summary:
        //     The up arrow key.
        Up = 265,
        //
        // Summary:
        //     The page up key.
        PageUp = 266,
        //
        // Summary:
        //     The page down key.
        PageDown = 267,
        //
        // Summary:
        //     The home key.
        Home = 268,
        //
        // Summary:
        //     The end key.
        End = 269,
        //
        // Summary:
        //     The caps lock key.
        CapsLock = 280,
        //
        // Summary:
        //     The scroll lock key.
        ScrollLock = 281,
        //
        // Summary:
        //     The num lock key.
        NumLock = 282,
        //
        // Summary:
        //     The print screen key.
        PrintScreen = 283,
        //
        // Summary:
        //     The pause key.
        Pause = 284,
        //
        // Summary:
        //     The F1 key.
        F1 = 290,
        //
        // Summary:
        //     The F2 key.
        F2 = 291,
        //
        // Summary:
        //     The F3 key.
        F3 = 292,
        //
        // Summary:
        //     The F4 key.
        F4 = 293,
        //
        // Summary:
        //     The F5 key.
        F5 = 294,
        //
        // Summary:
        //     The F6 key.
        F6 = 295,
        //
        // Summary:
        //     The F7 key.
        F7 = 296,
        //
        // Summary:
        //     The F8 key.
        F8 = 297,
        //
        // Summary:
        //     The F9 key.
        F9 = 298,
        //
        // Summary:
        //     The F10 key.
        F10 = 299,
        //
        // Summary:
        //     The F11 key.
        F11 = 300,
        //
        // Summary:
        //     The F12 key.
        F12 = 301,
        //
        // Summary:
        //     The F13 key.
        F13 = 302,
        //
        // Summary:
        //     The F14 key.
        F14 = 303,
        //
        // Summary:
        //     The F15 key.
        F15 = 304,
        //
        // Summary:
        //     The F16 key.
        F16 = 305,
        //
        // Summary:
        //     The F17 key.
        F17 = 306,
        //
        // Summary:
        //     The F18 key.
        F18 = 307,
        //
        // Summary:
        //     The F19 key.
        F19 = 308,
        //
        // Summary:
        //     The F20 key.
        F20 = 309,
        //
        // Summary:
        //     The F21 key.
        F21 = 310,
        //
        // Summary:
        //     The F22 key.
        F22 = 311,
        //
        // Summary:
        //     The F23 key.
        F23 = 312,
        //
        // Summary:
        //     The F24 key.
        F24 = 313,
        //
        // Summary:
        //     The F25 key.
        F25 = 314,
        //
        // Summary:
        //     The 0 key on the key pad.
        KeyPad0 = 320,
        //
        // Summary:
        //     The 1 key on the key pad.
        KeyPad1 = 321,
        //
        // Summary:
        //     The 2 key on the key pad.
        KeyPad2 = 322,
        //
        // Summary:
        //     The 3 key on the key pad.
        KeyPad3 = 323,
        //
        // Summary:
        //     The 4 key on the key pad.
        KeyPad4 = 324,
        //
        // Summary:
        //     The 5 key on the key pad.
        KeyPad5 = 325,
        //
        // Summary:
        //     The 6 key on the key pad.
        KeyPad6 = 326,
        //
        // Summary:
        //     The 7 key on the key pad.
        KeyPad7 = 327,
        //
        // Summary:
        //     The 8 key on the key pad.
        KeyPad8 = 328,
        //
        // Summary:
        //     The 9 key on the key pad.
        KeyPad9 = 329,
        //
        // Summary:
        //     The decimal key on the key pad.
        KeyPadDecimal = 330,
        //
        // Summary:
        //     The divide key on the key pad.
        KeyPadDivide = 331,
        //
        // Summary:
        //     The multiply key on the key pad.
        KeyPadMultiply = 332,
        //
        // Summary:
        //     The subtract key on the key pad.
        KeyPadSubtract = 333,
        //
        // Summary:
        //     The add key on the key pad.
        KeyPadAdd = 334,
        //
        // Summary:
        //     The enter key on the key pad.
        KeyPadEnter = 335,
        //
        // Summary:
        //     The equal key on the key pad.
        KeyPadEqual = 336,
        //
        // Summary:
        //     The left shift key.
        LeftShift = 340,
        //
        // Summary:
        //     The left control key.
        LeftControl = 341,
        //
        // Summary:
        //     The left alt key.
        LeftAlt = 342,
        //
        // Summary:
        //     The left super key.
        LeftSuper = 343,
        //
        // Summary:
        //     The right shift key.
        RightShift = 344,
        //
        // Summary:
        //     The right control key.
        RightControl = 345,
        //
        // Summary:
        //     The right alt key.
        RightAlt = 346,
        //
        // Summary:
        //     The right super key.
        RightSuper = 347,
        //
        // Summary:
        //     The menu key.
        Menu = 348,
        //
        // Summary:
        //     The last valid key in this enum.
        LastKey = 348
    }

    internal class BaseInputModule
    {
        public BaseInputModule() { }

        public MouseState? Mouse { get; internal set; }
        public KeyboardState? Keyboard { get; internal set; }

        public Dictionary<MouseButton, bool> mouseStates = [];
        public Dictionary<MouseButton, bool> mousePressedStates = [];
        public Dictionary<MouseButton, bool> mouseReleasedStates = [];

        public Vector2 mouseDelta;
        public Vector2 mouseScrollDelta;
        public Vector2 mouseScroll;
        public Vector2 mousePosition;

        public Dictionary<Key, bool> keyStates = [];
        public Dictionary<Key, bool> keyPressedStates = [];
        public Dictionary<Key, bool> keyReleasedStates = [];
    }
}
