﻿using NLog;
using OpenGLGameEngine;
namespace GameEngineTest;

public class Program
{

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game",windowMode:WindowModes.Fullscreen);

        Game.Run();
        
    }
}