using System.Numerics;
using GLFW;
using NLog;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Inputs;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Actors;

public class TestCameraController : CameraActor
{
    private Vector3 inputDirection;
    private Vector3 inputRotation;
    private float speed = 3f;
    private float panSpeed = 3f;
    private Vector3 angles = new Vector3(0,0, 0).ToRad();

    Logger logger = LogManager.GetCurrentClassLogger();
    public override void Start()
    {
        var inputs = GameInput.CreateInputGroup("CameraController");
        inputs.AddAction("forward", new[] { Keys.W }, type: InputControlType.Held,
            callback: () => { inputDirection.Z = 1f; });
        inputs.AddAction("backward", new[] { Keys.S }, type: InputControlType.Held,
            callback: () => { inputDirection.Z = -1f; });
        inputs.AddAction("left", new[] { Keys.A }, type: InputControlType.Held, callback: () => { inputDirection.X = -1f; });
        inputs.AddAction("right", new[] { Keys.D }, type: InputControlType.Held, callback: () => { inputDirection.X = 1f; });
        inputs.AddAction("lookup", new[] { Keys.Up }, type: InputControlType.Held,
            callback: () => { inputRotation.Y = 1f; });
        inputs.AddAction("lookdown", new[] { Keys.Down }, type: InputControlType.Held,
            callback: () => { inputRotation.Y = -1f; });
        inputs.AddAction("lookleft", new[] { Keys.Left }, type: InputControlType.Held,
            callback: () => { inputRotation.X = -1f; });
        inputs.AddAction("lookright", new[] { Keys.Right }, type: InputControlType.Held,
            callback: () => { inputRotation.X = 1f; });
    }

    public override void DrawTick()
    {
        inputRotation *= panSpeed * GameTime.DeltaTime;
        angles -= inputRotation;
        transform.rotation = Quaternion.CreateFromYawPitchRoll(angles.X, angles.Y, angles.Z);
        transform.position += Vector3.Transform(inputDirection, transform.rotation) * speed * GameTime.DeltaTime;
        inputDirection = Vector3.Zero;
        inputRotation = Vector3.Zero;
    }
}