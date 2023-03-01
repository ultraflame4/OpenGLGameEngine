using System.Runtime.CompilerServices;
using NLog;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine;

public static class Extensions
{
    static Logger logger = LogManager.GetCurrentClassLogger();
    
    /// <summary>
    /// Returns the GameWorld instance of the World 
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public static GameWorld? ToGameWorld(this World? world,[CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
    {
        if (world?.GetType() == typeof(GameWorld))
        {
            return (GameWorld)world;
        }
        else
        {
            logger.Error("Failed get GameWorld from component! Component is not in a GameWorld! {File}:{Lineno}", filePath, lineNumber);
        }
        return null;
    }
}