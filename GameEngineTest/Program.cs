using OpenGLGameEngine;
namespace GameEngineTest;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine($"Game Engine Version: {Utils.VERSION}! initialising...");
        
        GameEngine.Init();
        GameEngine.Run();
        
    }
}