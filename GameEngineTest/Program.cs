using NLog;
using OpenGLGameEngine;
namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game",windowMode: WindowModes.Windowed);

        Game.Run();
        
    }
}