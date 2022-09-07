using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace OpenGLGameEngine;

public static class Utils
{
    public static string VERSION = "0.0.0-dev";
    
    public static LoggingConfiguration GetNLogConfig()
    {
        LoggingConfiguration config = new LoggingConfiguration();
        FileTarget fileTarget = new FileTarget("logfile"){FileName = $"OpenGLGameEngine_{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}.log"};

        
        var highlightRule = new ConsoleRowHighlightingRule();
        highlightRule.Condition = ConditionParser.ParseExpression("level == LogLevel.Info");
        highlightRule.ForegroundColor = ConsoleOutputColor.Green;
        
        var highlightRule2 = new ConsoleRowHighlightingRule();
        highlightRule2.Condition = ConditionParser.ParseExpression("level == LogLevel.Trace");
        highlightRule2.ForegroundColor = ConsoleOutputColor.Blue;
        
        var highlightRule3 = new ConsoleRowHighlightingRule();
        highlightRule3.Condition = ConditionParser.ParseExpression("level == LogLevel.Error");
        highlightRule3.ForegroundColor = ConsoleOutputColor.Red;
        
        var highlightRule4 = new ConsoleRowHighlightingRule();
        highlightRule4.Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal");
        highlightRule4.ForegroundColor = ConsoleOutputColor.White;
        highlightRule4.BackgroundColor = ConsoleOutputColor.Red;
        
        var highlightRule5 = new ConsoleRowHighlightingRule();
        highlightRule5.Condition = ConditionParser.ParseExpression("level == LogLevel.Warn");
        highlightRule5.ForegroundColor = ConsoleOutputColor.Yellow;
        
        ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget("logconsole") {
                Layout = "${longdate:universalTime=false} | ${logger} - ${level:uppercase=false}: ${message} ${exception}",
                RowHighlightingRules = { highlightRule,highlightRule2,highlightRule3,highlightRule4,highlightRule5 }
        };

        config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
        return config;
    }
}