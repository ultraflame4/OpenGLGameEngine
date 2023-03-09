using System.Numerics;
using GLFW;
using OpenGLGameEngine.Inputs;

namespace OpenGLGameEngine.Components;

public class CameraController : EntityScript
{
    private Vector3 direction;
    private float speed = 0.5f;

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
    }

    public override void Update()
    {
        Transform? transform = Entity?.GetComponent<Transform>();

        if (transform == null)
            return;
        transform.position += direction * speed;
        logger.Debug(transform.position);
        direction = Vector3.Zero;
    }

    public override void Draw() { }
}