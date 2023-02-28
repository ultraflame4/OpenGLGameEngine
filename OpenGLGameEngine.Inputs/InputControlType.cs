namespace OpenGLGameEngine.Inputs;

public enum InputControlType
{
    /// <summary>
    /// On the first frame where the key/button is pressed, input is active
    /// </summary>
    Press,
    /// <summary>
    /// AS long as the the key/button is held down / pressed, input is active
    /// </summary>
    Held,
    /// <summary>
    /// input is only active on the first frame when the key / button is released
    /// </summary>
    Release
}