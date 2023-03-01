using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace OpenGLGameEngine.Utils;

/// <summary>
///     A general utility class that contain information about the game engine and various methods used internally.
///     <br />
///     --- <b>Avoid trying to set or change any variables defined, or calling any methods in here!</b> ----
/// </summary>
public static class CoreUtils
{
    public static string VERSION = "0.0.0-dev";


    public static LoggingConfiguration GetNLogConfig()
    {
        var layout = "${longdate:universalTime=false} | ${level:uppercase=true:padding=-5} | " +
                     "${logger} : ${message} ${exception}";
        var config = new LoggingConfiguration();
        var fileTarget = new FileTarget("logfile") {
                FileName = "${basedir}/logs/" + $"OpenGLGameEngine_{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}.log",
                Layout = layout
        };

        var consoleTarget = new ColoredConsoleTarget("logconsole") {
                Layout = layout,
                RowHighlightingRules = {
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Trace"),
                                ForegroundColor = ConsoleOutputColor.Magenta
                        },
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Debug"),
                                ForegroundColor = ConsoleOutputColor.White
                        },
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                                ForegroundColor = ConsoleOutputColor.Green
                        },
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Warn"),
                                ForegroundColor = ConsoleOutputColor.Yellow
                        },
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                                ForegroundColor = ConsoleOutputColor.Red
                        },
                        new ConsoleRowHighlightingRule {
                                Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal"),
                                ForegroundColor = ConsoleOutputColor.Red
                        }
                }
        };

        config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
        return config;
    }

    public static void ConfigureNLog(Logger logger)
    {
        LogManager.Configuration = GetNLogConfig();
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var e = args.ExceptionObject as Exception;
            logger.Fatal(e, "A Fatal Unhandled Error has occured!------------------------------------------------\n");
        };
        logger.Info("Loaded logging configuration.");
    }
}