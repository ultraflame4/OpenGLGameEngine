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

        Game.Run();
    }
}