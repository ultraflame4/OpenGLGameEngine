using GLFW;
using NLog;
using OpenGLGameEngine;
using OpenGLGameEngine.Inputs;

namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed);
        InputActionScheme test = Game.CreateInputActionScheme("TEST ACTION SCHEME");

        var c = test.addAction(
            "A",
            keyInputs: new[] { Keys.A,Keys.B},
            mouseInputs: new[] { MouseButton.Button1 },
            type: InputControlType.Release
        );
        c.WhenInputActive += () => { logger.Debug("A RELEASED"); };
        Game.Run();
    }
}