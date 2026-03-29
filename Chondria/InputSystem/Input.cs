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

        public static void UpdateInput(MouseState mouse, KeyboardState keyboard)
        {
            module.Mouse = mouse;
            module.Keyboard = keyboard;

            for (int i = 0; i < Enum.GetNames<MouseButton>().Length; i++)
            {
                var btn = (MouseButton)i;

                module.mouseStates[btn] = mouse.IsButtonDown(btn);

                module.mousePressedStates[btn] = mouse.IsButtonPressed(btn);

                module.mouseReleasedStates[btn] = mouse.IsButtonReleased(btn);

            }

            module.mouseDelta = mouse.Delta.ChonVec();
            MouseDelta = module.mouseDelta;

            module.mouseScrollDelta = mouse.ScrollDelta.ChonVec();
            ScrollDelta = module.mouseScrollDelta;

            module.mouseScroll = mouse.Scroll.ChonVec();
            Scroll = module.mouseScroll;

            module.mousePosition = mouse.Position.ChonVec();
            MousePosition = module.mousePosition;

            for (int i = 0; i < Enum.GetNames<Keys>().Length; i++)
            {
                var key = (Keys)i;

                module.keyStates[key] = keyboard.IsKeyDown(key);

                module.keyPressedStates[key] = keyboard.IsKeyPressed(key);

                module.keyReleasedStates[key] = keyboard.IsKeyReleased(key);

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
        public static bool IsKeyDown(Keys key) => module.keyStates[key];

        /// <summary>Gets the keys that was just pressed.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just pressed and false otherwise</returns>
        public static bool WasKeyPressed(Keys key) => module.keyPressedStates[key];

        /// <summary>Gets the keys that was just released.</summary>
        /// <param name="key">The key</param>
        /// <returns>True if the key was just released and false otherwise</returns>
        public static bool WasKeyReleased(Keys key) => module.keyReleasedStates[key];

        /// <summary>Gets the keys that is down.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key is down and false otherwise</returns>
        public static bool IsKeyDown(string key)
        {
            if (Enum.TryParse<Keys>(key, true, out var parsedKey))
                return module.keyStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets the keys that was just pressed.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just pressed and false otherwise</returns>
        public static bool WasKeyPressed(string key)
        {
            if (Enum.TryParse<Keys>(key, true, out var parsedKey))
                return module.keyPressedStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets the keys that was just released.</summary>
        /// <param name="key">The key name</param>
        /// <returns>True if the key was just released and false otherwise</returns>
        public static bool WasKeyReleased(string key)
        {
            if (Enum.TryParse<Keys>(key, true, out var parsedKey))
                return module.keyReleasedStates.TryGetValue(parsedKey, out var state) && state;
            return false;
        }

        /// <summary>Gets all the current keys that are being pressed.</summary>
        /// <returns>Keys[] a list of the pressed keys.</returns>
        public static Keys[] AllKeysDown()
        {
            List<Keys> result = [];
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
            "Horizontal" => (IsKeyDown(Keys.A) ? 1f : 0f) - (IsKeyDown(Keys.D) ? 1f : 0f),
            "Vertical" => (IsKeyDown(Keys.W) ? 1f : 0f) - (IsKeyDown(Keys.S) ? 1f : 0f),
            "MouseX" => MousePosition.X,
            "MouseY" => MousePosition.Y,
            _ => 0f
        };
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

        public Dictionary<Keys, bool> keyStates = [];
        public Dictionary<Keys, bool> keyPressedStates = [];
        public Dictionary<Keys, bool> keyReleasedStates = [];
    }
}
