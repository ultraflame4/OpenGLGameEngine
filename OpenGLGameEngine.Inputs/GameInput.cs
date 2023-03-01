using NLog;

namespace OpenGLGameEngine.Inputs;

/// <summary>
///     The class that manages the InputActionGroup(s)
/// </summary>
public static class GameInput
{
    private static readonly List<InputActionGroup> inputGroups = new();
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();


    /// <summary>
    ///     Creates and add an Input action scheme
    /// </summary>
    /// <param name="name">The name of the scheme</param>
    /// <returns>Returns the newly created instance of <see cref="InputActionGroup" /></returns>
    public static InputActionGroup CreateInputGroup(string name)
    {
        var scheme = new InputActionGroup(name);
        inputGroups.Add(scheme);
        logger.Debug($"Created new input action scheme {name}");
        return scheme;
    }

    /// <summary>
    ///     Returns the input action group with the specified name
    /// </summary>
    /// <param name="name">The name of the Input Action Scheme</param>
    /// <returns></returns>
    public static InputActionGroup? GetInputGroup(string name)
    {
        return inputGroups.Find(scheme => scheme.name == name);
    }

    /// <summary>
    ///     Returns an array copy of the list of input action groups created
    /// </summary>
    /// <returns></returns>
    public static InputActionGroup[] GetAllInputGroups()
    {
        return inputGroups.ToArray();
    }
}