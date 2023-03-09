using System.Diagnostics;
using System.Numerics;
using GLFW;
using OpenGLGameEngine.Inputs;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Components;

public class CameraController : EntityScript
{
    private Vector3 direction;
    private Vector3 rotation;
    private float speed = 5f;
    private float panSpeed = 1f;

    public override void Start()
    {
        var inputs = GameInput.CreateInputGroup("CameraController");
        inputs.AddAction("forward", new[] { Keys.W }, type: InputControlType.Held, callback: () =>
        {
            if (direction.Z < 1f) direction.Z += 1f;
        });
        inputs.AddAction("backward", new[] { Keys.S }, type: InputControlType.Held, callback: () =>
        {
            if (direction.Z > -1f) direction.Z -= 1f;
        });
        inputs.AddAction("left", new[] { Keys.A }, type: InputControlType.Held, callback: () =>
        {
            if (direction.X > -1f) direction.X -= 1f;
        });
        inputs.AddAction("right", new[] { Keys.D }, type: InputControlType.Held, callback: () =>
        {
            if (direction.X < 1f) direction.X += 1f;
        });
        
        inputs.AddAction("lookup", new[] { Keys.Up }, type: InputControlType.Held, callback: () =>
        {
            if (rotation.Y > -1f) rotation.Y -= 1f;
        });
        inputs.AddAction("lookdown", new[] { Keys.Down }, type: InputControlType.Held, callback: () =>
        {
            if (rotation.Y < 1f) rotation.Y += 1f;
        });
        inputs.AddAction("lookleft", new[] { Keys.Left }, type: InputControlType.Held, callback: () =>
        {
            if (rotation.X > -1f) rotation.X -= 1f;
        });
        inputs.AddAction("lookright", new[] { Keys.Right }, type: InputControlType.Held, callback: () =>
        {
            if (rotation.X < 1f) rotation.X += 1f;
        });
    }

    public override void Update()
    {
        Transform? transform = Entity?.GetComponent<Transform>();

        if (transform == null)
            return;


        transform.rotation += rotation * panSpeed * GameTime.DeltaTime;
        transform.position += transform.rotation * direction.Z  * speed * GameTime.DeltaTime;
        
        direction = Vector3.Zero;
        rotation=Vector3.Zero;
    }

    public override void Draw() { }
}