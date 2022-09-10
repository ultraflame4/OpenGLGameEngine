using NLog;

namespace OpenGLGameEngine.Inputs;

public static class InputActionSchemeManager
{
    private static List<InputActionScheme> inputActionSchemes = new List<InputActionScheme>();
    static Logger logger = LogManager.GetCurrentClassLogger();
    

    /// <summary>
    /// Creates and add an Input action scheme
    /// </summary>
    /// <param name="name">The name of the scheme</param>
    /// <returns>Returns the newly created instance of <see cref="InputActionScheme"/></returns>
    public static InputActionScheme CreateScheme(string name)
    {
        var scheme = new InputActionScheme(name);
        inputActionSchemes.Add(scheme);
        logger.Debug($"Created new input action scheme {name}");
        return scheme;
    }

    /// <summary>
    /// Returns the input action scheme with the specified name
    /// </summary>
    /// <param name="name">The name of the Input Action Scheme</param>
    /// <returns></returns>
    public static InputActionScheme? GetScheme(string name)
    {
        return inputActionSchemes.Find(scheme => scheme.name == name);
    }

    /// <summary>
    /// Returns an array copy of the list of input action schemes created
    /// </summary>
    /// <returns></returns>
    public static InputActionScheme[] GetInputActionSchemes()
    {
        return inputActionSchemes.ToArray();
    }
}