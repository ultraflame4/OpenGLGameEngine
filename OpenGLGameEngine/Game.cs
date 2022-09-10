using GLFW;
using OpenGLGameEngine.Inputs;

namespace OpenGLGameEngine;

/// <summary>
/// The main class to interact with. It contains methods that will be used in creation of the game.
/// <br/>
/// </summary>
public static class Game
{
    
    /// <summary>
    /// Initialises the game engine and creates the window
    /// </summary>
    /// <param name="windowTitle">The title of the window</param>
    /// <param name="fullscreenKey">The key to toggle fullscreen when pressed. Set to null to disable</param>
    /// <param name="windowMode">The display mode: windowed, maximised, fullscreen, fullscreen borderless.</param>
    /// <param name="windowSize">Size of the window in windowed mode.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Create(
        string windowTitle,
        Nullable<Keys> fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed,
        (int width, int height) windowSize = default
        )
    {
        GameLoop.Create(windowTitle,fullscreenKey,windowMode,windowSize);
    }
    public static void Run()
    {
        GameLoop.Run();
    }

    /// <summary>
    /// Creates and add an Input action scheme
    /// </summary>
    /// <param name="name">The name of the scheme</param>
    /// <returns>Returns the newly created instance of <see cref="InputActionScheme"/></returns>
    public static InputActionScheme CreateInputActionScheme(string name)
    {
        return InputActionSchemeManager.CreateScheme(name);
    }

    /// <summary>
    /// Returns the input action scheme with the specified name
    /// </summary>
    /// <param name="name">The name of the Input Action Scheme</param>
    /// <returns></returns>
    public static InputActionScheme? GetInputActionScheme(string name)
    {
        return InputActionSchemeManager.GetScheme(name);
    }

    /// <summary>
    /// Returns an array copy of the list of input action schemes created
    /// </summary>
    /// <returns></returns>
    public static InputActionScheme[] GetInputActionSchemes()
    {
        return InputActionSchemeManager.GetInputActionSchemes();
    }
}