using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace OpenGLGameEngine.Utils;

/// <summary>
/// A general utility class that should only be used by the game engine!
/// </summary>
public static class Utils
{
    public static string VERSION = "0.0.0-dev";

    private static LoggingConfiguration logging_config;

    public static LoggingConfiguration GetNLogConfig()
    {
        if (logging_config != null)
        {
            return logging_config;
        }

        string layout = "${longdate:universalTime=false} | ${level:uppercase=true:padding=-5} | ${logger} : ${message} ${exception}";
        LoggingConfiguration config = new LoggingConfiguration();
        FileTarget fileTarget = new FileTarget("logfile") {
                FileName = "${basedir}/logs/" + $"OpenGLGameEngine_{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}.log",
                Layout = layout
        };

        ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget("logconsole") {
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
                                ForegroundColor = ConsoleOutputColor.White,
                                BackgroundColor = ConsoleOutputColor.Red
                        }
                }
        };

        config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
        logging_config = config;
        return logging_config;
    }
}