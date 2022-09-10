using GLFW;

namespace OpenGLGameEngine.Inputs;

/// <summary>
/// Defines a group of input actions (controls) that can be easily enabled or disabled.
/// <br/>
/// The recommended way to get input for controls.
/// <br/>
/// <b>NOTE: This class should never be instantiated by it self. USE : <see cref="Game.CreateInputActionScheme"/></b>
/// </summary>
public class InputActionScheme
{
    private readonly string _name;
    public bool enabled = true;
    private List<InputAction> controls = new List<InputAction>();

    /// <summary>
    /// Defines a group of input actions (controls) that can be easily enabled or disabled.
    /// </summary>
    /// <param name="name">The name of scheme. i.e 'Inventory Controls'</param>
    public InputActionScheme(string name)
    {
        _name = name;
    }


    /// <summary>
    /// Creates a new action that activates when the specificed keys and mouse buttons are pressed
    /// <br/>
    /// Note: <b>All mouse inputs will only be read and processed after the key combination specified has matched.! This may lead to weird behaviours</b>
    /// </summary>
    /// <param name="name">The name of the action. May be the display name.</param>
    /// <param name="keyInputs">The combination of keyboard key inputs. (Using enums). Example: Keys.W|Keys.A = W + A keys</param>
    /// <param name="mouseInputs">The combination of mouse inputs. Using enums. same as above</param>
    /// <param name="type">How the action should activate. Refer to the docstring for <see cref="InputState"/></param>
    /// <param name="callback">The callback to call when action is active. Can be added later using <see cref="InputAction.WhenInputActive"/> event</param>
    /// <returns>Returns the newly create input action instance</returns>
    public InputAction addAction(
        string name,
        Keys[]? keyInputs = null,
        MouseButton[]? mouseInputs = null,
        InputControlType type = InputControlType.Press,
        InputAction.InputActionHandler? callback = null
    )
    {
        var inputControl = new InputAction(this, name, keyInputs, mouseInputs, type);
        controls.Add(inputControl);
        if (callback != null) inputControl.WhenInputActive += callback;

        return inputControl;
    }
}