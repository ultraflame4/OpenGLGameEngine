using System.Runtime.InteropServices;
using NLog;
using Silk.NET.Maths;
using Silk.NET.Windowing;
namespace OpenGLGameEngine;

public class Game
{
    static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    static Game instance = null;
    private IWindow window;

    static Game()
    {
        LogManager.Configuration = Utils.GetNLogConfig();
        logger.Info($"Loaded logging configuration.");
        logger.Info($"GameEngine version: {Utils.VERSION}");
    }
    
    private Game(string windowTitle)
    {
        //Create a window.
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = windowTitle;
        window = Window.Create(options);
        
    }

    public static Game Create(string windowTitle)
    {
        if (instance is null)
        {
            logger.Info($"beginning initialisation and creation...");
            logger.Info($"Set Window Title: {windowTitle}");
            instance = new Game(windowTitle);
        }
        else
        {
            logger.Warn("An instance of this class already exists! Skipping..");
        }

        return instance;
    }

    public static Game getInstance()
    {
        if (instance is null)
        {
            logger.Warn("There is no existing instance of this class! Will Return Null !!!");
        }
        return instance;
    }


    public void Run()
    {
        
    }
}