﻿using GLFW;
using NLog;

namespace OpenGLGameEngine.Inputs;

/// <summary>
///     A class to handle mouse and keyboard event callbacks from Glfw.
///     <br />
///     Includes several related events and functions.
///     <br />
///     For proper inputs controls, look at <see cref="GameInput.CreateInputGroup" />
/// </summary>
public static class KeyboardMouseInput
{
    /// <summary>
    ///     The standard callback handler type for keyboard input events.
    /// </summary>
    /// <param name="key">The key that was pressed/released</param>
    /// <param name="scanCode">The scan code for the key that was pressed/released. This is platform-specific</param>
    /// <param name="state">The state of the key. Use .IsStatePressed() to convert to boolean</param>
    /// <param name="mods">The combination of modifier keys that were pressed. Use mods.HasFlag to check for specific keys.</param>
    public delegate void OnKeyEventHandler(Keys key, int scanCode, InputState state, ModifierKeys mods);

    public delegate void OnMouseBtnEventHandler(MouseButton btn, InputState state, ModifierKeys mods);

    private static Window window;
    private static Logger logger = LogManager.GetCurrentClassLogger();


    // have to manually create delegate instances that are assigned to a variable.
    // so that the garbage collector does not collect it. https://github.com/ForeverZer0/glfw-net/issues/44#issuecomment-927222681
    // glfw bindings is unmanaged code, so any instance created automatically and used for callback will not have a reference count.
    // hence variable is needed else GC will collect the instance and result in errors!
    private static readonly KeyCallback _keyCallback = OnGlfwKeyCallback;
    private static readonly MouseButtonCallback _mouseBtnCallback = OnGlfwMouseButtonCallback;

    private static readonly HashSet<Keys> heldKeys = new();

    // There is no repeat state for mouse buttons, so we have to implement it ourself.
    private static readonly HashSet<MouseButton> heldMouseBtns = new();

    /// <summary>
    ///     This event fires everytime a key is pressed or released. Will only be fired ONCE for that frame when it was pressed
    /// </summary>
    public static event OnKeyEventHandler? OnKeyUpDown;

    /// <summary>
    ///     This event fires when the key is pressed. Will only be fired ONCE for that frame when it was pressed
    /// </summary>
    public static event OnKeyEventHandler? OnKeyDown;

    /// <summary>
    ///     This event fires when the key is released.
    /// </summary>
    public static event OnKeyEventHandler? OnKeyUp;

    /// <summary>
    ///     This event will fire continuously as long as a key is held. Please note that there is a delay before it starts firing continuously.
    ///     <br />
    ///     This is recommended for text inputs
    /// </summary>
    public static event OnKeyEventHandler? OnKeyRepeat;

    /// <summary>
    ///     This event will fire continuously as long as a key is held. Unlike <see cref="OnKeyRepeat" />, there is no delay.
    /// </summary>
    public static event OnKeyEventHandler? OnKeyHeld;

    /// <summary>
    ///     Fires on any key events.
    /// </summary>
    public static event OnKeyEventHandler? OnAllKey;

    public static void Init(Window _window)
    {
        window = _window;
        Glfw.SetKeyCallback(window, _keyCallback);
        Glfw.SetMouseButtonCallback(window, _mouseBtnCallback);
    }

    private static bool IsStatePressed(this InputState state)
    {
        return state is InputState.Press or InputState.Repeat;
    }

    /// <summary>
    ///     Checks if a key is currently being held/pressed.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key is currently pressed. False if not.</returns>
    public static bool IsKeyPressed(Keys key)
    {
        return Glfw.GetKey(window, key).IsStatePressed();
    }

    /// <summary>
    ///     Checks if a mouse button is currently being held/pressed.
    /// </summary>
    /// <param name="btn">The mouse button to check</param>
    /// <returns>True if the key is currently pressed. False if not.</returns>
    public static bool IsMouseButton(MouseButton btn)
    {
        return Glfw.GetMouseButton(window, btn).IsStatePressed();
    }

    private static void OnGlfwKeyCallback(IntPtr intPtr, Keys key, int scanCode, InputState state, ModifierKeys mods)
    {
        OnAllKey?.Invoke(key, scanCode, state, mods);
        switch (state)
        {
            case InputState.Release:

                OnKeyUp?.Invoke(key, scanCode, state, mods);
                OnKeyUpDown?.Invoke(key, scanCode, state, mods);
                heldKeys.Remove(key);
                break;
            case InputState.Press:
                heldKeys.Add(key);
                OnKeyDown?.Invoke(key, scanCode, state, mods);
                OnKeyUpDown?.Invoke(key, scanCode, state, mods);
                OnKeyHeld?.Invoke(key, scanCode, state, mods);
                OnKeyRepeat?.Invoke(key, scanCode, state, mods);
                break;
            case InputState.Repeat:
                OnKeyRepeat?.Invoke(key, scanCode, state, mods);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    /// <summary>
    ///     This event fires when the mouse button is pressed. Will only be fired ONCE for that frame when it was pressed
    /// </summary>
    public static event OnMouseBtnEventHandler? OnMouseButtonDown;

    /// <summary>
    ///     This event fires when the mouse button is released.
    /// </summary>
    public static event OnMouseBtnEventHandler? OnMouseButtonUp;

    /// <summary>
    ///     This event fires as long as a mouse button is held continuously.
    /// </summary>
    public static event OnMouseBtnEventHandler? OnMouseButtonHeld;

    /// <summary>
    ///     This event fires on any mouse button events
    /// </summary>
    public static event OnMouseBtnEventHandler? OnAllMouseButton;

    private static void OnGlfwMouseButtonCallback(IntPtr intPtr, MouseButton btn, InputState state, ModifierKeys mods)
    {
        OnAllMouseButton?.Invoke(btn, state, mods);
        switch (state)
        {
            case InputState.Release:
                OnMouseButtonUp?.Invoke(btn, state, mods);
                heldMouseBtns.Remove(btn);
                break;
            case InputState.Press:
                OnMouseButtonDown?.Invoke(btn, state, mods);
                OnMouseButtonHeld?.Invoke(btn, InputState.Repeat, mods);
                heldMouseBtns.Add(btn);
                break;
            case InputState.Repeat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public static void Update()
    {
        foreach (var mouseBtn in heldMouseBtns) OnMouseButtonHeld?.Invoke(mouseBtn, InputState.Repeat, 0);

        foreach (var key in heldKeys) OnKeyHeld?.Invoke(key, Glfw.GetKeyScanCode(key), InputState.Repeat, 0);
    }
}