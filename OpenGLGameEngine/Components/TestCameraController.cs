using System.Diagnostics;
using System.Numerics;
using GLFW;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Inputs;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Components;

public class TestCameraController : EntityScript
{
    private Vector3 direction;
    private Vector3 rotation;
    private float speed = 5f;
    private float panSpeed = 30f;

    public override void Start()
    {
        var inputs = GameInput.CreateInputGroup("CameraController");
        inputs.AddAction("forward", new[] { Keys.W }, type: InputControlType.Held,callback: () => { direction.Z = 1f; });
        inputs.AddAction("backward", new[] { Keys.S }, type: InputControlType.Held,callback: () => { direction.Z = -1f; });
        inputs.AddAction("left", new[] { Keys.A }, type: InputControlType.Held, callback: () => { direction.X = -1f; });
        inputs.AddAction("right", new[] { Keys.D }, type: InputControlType.Held, callback: () => { direction.X = 1f; });
        inputs.AddAction("lookup", new[] { Keys.Up }, type: InputControlType.Held,callback: () => { rotation.Y = 1f; });
        inputs.AddAction("lookdown", new[] { Keys.Down }, type: InputControlType.Held,callback: () => { rotation.Y = -1f; });
        inputs.AddAction("lookleft", new[] { Keys.Left }, type: InputControlType.Held,callback: () => { rotation.X = -1f; });
        inputs.AddAction("lookright", new[] { Keys.Right }, type: InputControlType.Held,callback: () => { rotation.X = 1f; });
    }

    public override void Update()
    {
        Transform? transform = Entity?.GetComponent<Transform>();

        if (transform == null)
            return;


        rotation *= panSpeed * GameTime.DeltaTime;

        transform.rotation.RotateEuler(rotation);

        transform.position += Vector3.Transform(direction, transform.rotation) * speed * GameTime.DeltaTime;

        direction = Vector3.Zero;
        rotation = Vector3.Zero;
    }

    public override void Draw() { }
}