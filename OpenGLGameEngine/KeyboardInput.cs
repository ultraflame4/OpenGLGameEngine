using GLFW;
using NLog;

namespace OpenGLGameEngine;

public static class KeyboardInput
{
    static Window window;
    static Logger logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// The standard callback handler type for keyboard input events.
    /// </summary>
    /// <param name="key">The key that was pressed/released</param>
    /// <param name="scanCode">The scan code for the key that was pressed/released. This is platform-specific</param>
    /// <param name="state">The state of the key. Use .IsStatePressed() to convert to boolean</param>
    /// <param name="mods">The combination of modifier keys that were pressed. Use mods.HasFlag to check for specific keys.</param>
    public delegate void OnKeyEventHandler(Keys key, int scanCode, InputState state, ModifierKeys mods);

    /// <summary>
    /// This event fires everytime a key is pressed or released. Will only be fired ONCE for that frame when it was pressed
    /// </summary>
    public static event OnKeyEventHandler OnKeyUpDown;

    /// <summary>
    /// This event fires when the key is pressed. Will only be fired ONCE for that frame when it was pressed
    /// </summary>
    public static event OnKeyEventHandler OnKeyDown;

    /// <summary>
    /// This event fires when the key is released.
    /// </summary>
    public static event OnKeyEventHandler OnKeyUp;

    /// <summary>
    /// This event will fire continuously as long as a key is held. 
    /// </summary>
    public static event OnKeyEventHandler OnKeyHeld;

    /// <summary>
    /// Like OnKey, receives both pressed and released events, but will be fired multiple times
    /// </summary>
    public static event OnKeyEventHandler OnAllKey;


    public static void Init(Window _window)
    {
        window = _window;
        Glfw.SetKeyCallback(window, OnGlfwKeyCallback);
    }

    private static bool IsStatePressed(this InputState state)
    {
        return state is InputState.Press or InputState.Repeat;
    }

    /// <summary>
    /// Checks if a key is currently being held/pressed.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is currently pressed. False if not.</returns>
    public static bool IsKeyPressed(Keys key)   
    {
        return Glfw.GetKey(window, key).IsStatePressed();
    }

    static void OnGlfwKeyCallback(IntPtr intPtr, Keys key, int scanCode, InputState state, ModifierKeys mods)
    {
        OnAllKey?.Invoke(key, scanCode, state, mods);
        switch (state)
        {
            case InputState.Release:
                OnKeyUp?.Invoke(key,scanCode,state,mods);
                OnKeyUpDown?.Invoke(key,scanCode,state,mods);
                break;
            case InputState.Press:
                OnKeyDown?.Invoke(key,scanCode,state,mods);
                OnKeyUpDown?.Invoke(key,scanCode,state,mods);
                OnKeyHeld?.Invoke(key,scanCode,state,mods);
                break;
            case InputState.Repeat:
                OnKeyHeld?.Invoke(key,scanCode,state,mods);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}