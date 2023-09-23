﻿using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Core.Windowing;
using ErrorCode = GLFW.ErrorCode;
using Window = OpenGLGameEngine.Core.Windowing.Window;

namespace OpenGLGameEngine.Core;

/// <summary>
///     The class containing the game loop and creation of window.
///     <br />
///     <br />
///     * portions of the code were modified and copied from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6
/// </summary>
public class MainWindow
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public static Window? window { get; private set; }
    

    static MainWindow()
    {
        CoreUtils.ConfigureNLog(logger);
        logger.Info($"GameEngine version: {CoreUtils.VERSION}");
    }

    /// <summary>
    ///     This event is invoked when the game loops exits and the window closes.
    /// </summary>
    public static event Action? GameLoopExit;
    public static event Action? GameLoopDraw;
    public static event Action? GameLoopUpdate;


    /// <summary>
    ///     Initialises the game engine and creates the window
    /// </summary>
    /// <param name="windowTitle">The title of the window</param>
    /// <param name="fullscreenKey">The key to toggle fullscreen when pressed. Set to null to disable</param>
    /// <param name="windowMode">The display mode: windowed, maximised, fullscreen, fullscreen borderless.</param>
    /// <param name="windowSize">Size of the window in windowed mode.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Create(
        string windowTitle,
        Keys? fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed,
        (int width, int height) windowSize = default
    )
    {
        if (windowSize.Equals(default)) windowSize = WindowUtils.DefaultWindowSize;


        // Start initialisation and create opengl context and window.
        WindowManager.Init();

        // Window and context creation
        logger.Debug("Begin window and context creation...");
        window = Window.Create(windowTitle, new WindowRect(0,0, windowSize.width,windowSize.height), windowMode, true);
        window.Init();
        window.InitInput();
        logger.Info("!!!!!!!!!!!!!!!!!!!!! Initial configuration and initialisation done !!!!!!!!!!!!!!!!!!!!!");
    }


    public static void Run()
    {
        
        while (!window.shouldClose)
        {
            GameTime.UpdateDeltaTime(Glfw.Time);

            window.Poll();

            // Update game logic
            GameLoopUpdate?.Invoke();
            
            GameLoopDraw?.Invoke();
            
            window.SwapBuffers();
        }
        Stop();
    }
    
    private static void Stop()
    {
        logger.Info("Exited game loop!");
        logger.Debug("Invoking GameLoopExit events...");
        GameLoopExit?.Invoke();
        logger.Debug("Event invocation success. Terminating Glfw...");
        WindowManager.Terminate();
        logger.Info("Game shutdown and exited successfully.");
    }
}