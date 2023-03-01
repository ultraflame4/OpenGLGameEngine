using GLFW;

namespace OpenGLGameEngine.Inputs;

public class InputAction
{
    public delegate void InputActionHandler();

    private readonly string name;
    private bool active;
    private readonly Keys[]? keyInputs;

    private bool last_all_inputs_active;
    private readonly MouseButton[]? mouseInputs;
    private readonly InputActionGroup scheme;
    private InputControlType type;

    /// <summary>
    ///     Creates a new action that activates when the specificed keys and mouse buttons are pressed
    ///     <br />
    ///     Note: <b>All mouse inputs will only be read and processed after the key combination specified has matched.! This may lead to weird behaviours</b>
    /// </summary>
    /// <param name="scheme">The <see cref="InputActionGroup" /> this InputAction is attached to.</param>
    /// <param name="name">The name of the action. May be the display name.</param>
    /// <param name="keyInputs">The combination of keyboard key inputs.</param>
    /// <param name="mouseInputs">The combination of mouse inputs. </param>
    /// <param name="type">How the action should activate. Refer to the docstring for <see cref="InputState" /></param>
    /// <returns></returns>
    public InputAction(InputActionGroup scheme, string name, Keys[]? keyInputs = null, MouseButton[]? mouseInputs = null, InputControlType type = InputControlType.Press)
    {
        this.scheme = scheme;
        this.name = name;
        this.keyInputs = keyInputs;
        this.mouseInputs = mouseInputs;
        this.type = type;
        active = false;

        // Register the respective handler to the various respective events
        if (keyInputs is not null)
            switch (type)
            {
                case InputControlType.Press:
                    KeyboardMouseInput.OnKeyDown += OnKeyInputHandler;
                    break;
                case InputControlType.Held:
                    KeyboardMouseInput.OnKeyHeld += OnKeyInputHandler;
                    break;
                case InputControlType.Release:
                    KeyboardMouseInput.OnKeyDown += OnKeyInputHandler; // required to check if all keys were pressed before hand
                    KeyboardMouseInput.OnKeyUp += OnKeyInputHandler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        if (mouseInputs is not null)
            switch (type)
            {
                case InputControlType.Press:
                    KeyboardMouseInput.OnMouseButtonDown += OnMouseBtnInputHandler;
                    break;
                case InputControlType.Held:
                    KeyboardMouseInput.OnMouseButtonHeld += OnMouseBtnInputHandler;
                    break;
                case InputControlType.Release:
                    KeyboardMouseInput.OnMouseButtonDown += OnMouseBtnInputHandler; // required to check if all keys were pressed before hand
                    KeyboardMouseInput.OnMouseButtonUp += OnMouseBtnInputHandler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
    }

    /// <summary>
    ///     This event fires whenever the input is active.
    /// </summary>
    public event InputActionHandler WhenInputActive;

    private void OnKeyInputHandler(Keys key, int scanCode, InputState state, ModifierKeys mods)
    {
        onInputEventUpdate();
    }

    public void OnMouseBtnInputHandler(MouseButton btn, InputState state, ModifierKeys mods)
    {
        onInputEventUpdate();
    }

    ~InputAction()
    {
        if (keyInputs is not null)
            switch (type)
            {
                case InputControlType.Press:
                    KeyboardMouseInput.OnKeyDown -= OnKeyInputHandler;
                    break;
                case InputControlType.Held:
                    KeyboardMouseInput.OnKeyHeld -= OnKeyInputHandler;
                    break;
                case InputControlType.Release:
                    KeyboardMouseInput.OnKeyDown -= OnKeyInputHandler;
                    KeyboardMouseInput.OnKeyUp -= OnKeyInputHandler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        if (mouseInputs is not null)
            switch (type)
            {
                case InputControlType.Press:
                    KeyboardMouseInput.OnMouseButtonDown -= OnMouseBtnInputHandler;
                    break;
                case InputControlType.Held:
                    KeyboardMouseInput.OnMouseButtonHeld -= OnMouseBtnInputHandler;
                    break;
                case InputControlType.Release:
                    KeyboardMouseInput.OnMouseButtonDown -= OnMouseBtnInputHandler;
                    KeyboardMouseInput.OnMouseButtonUp -= OnMouseBtnInputHandler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
    }

    public void onInputEventUpdate()
    {
        if (!scheme.enabled) return;
        var all_inputs_active = true; // temp set to true.
        // if any input not active, set to false
        if (keyInputs != null)
            foreach (var keyInput in keyInputs)
                if (!KeyboardMouseInput.IsKeyPressed(keyInput))
                {
                    all_inputs_active = false;
                    break;
                }

        if (mouseInputs != null)
            foreach (var mouseBtn in mouseInputs)
                if (!KeyboardMouseInput.IsMouseButton(mouseBtn))
                {
                    all_inputs_active = false;
                    break;
                }

        // If all inputs active, make active
        if (type is InputControlType.Press or InputControlType.Held && all_inputs_active)
        {
            active = true;
            WhenInputActive?.Invoke();
        }

        // if type is release and last time all input active but not now, make active 
        if (type is InputControlType.Release && last_all_inputs_active && !all_inputs_active)
        {
            active = true;
            WhenInputActive?.Invoke();
        }


        last_all_inputs_active = all_inputs_active;
    }


    public bool IsActive()
    {
        return active;
    }
}