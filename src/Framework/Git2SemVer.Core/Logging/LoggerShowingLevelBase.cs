using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Logging;

public abstract class LoggerShowingLevelBase : LoggerBase
{
    public override LoggingLevel Level { get; set; }

    public void Log(LoggingLevel level, string message)
    {
        if (Level < level)
        {
            return;
        }

        var levelPrefix = new Dictionary<LoggingLevel, string>
        {
            { LoggingLevel.Trace, "TRACE | " },
            { LoggingLevel.Debug, "DEBUG | " },
            { LoggingLevel.Info, "INFO  | " },
            { LoggingLevel.Warning, "WARN  | " },
            { LoggingLevel.Error, "ERROR | " }
        };
        const string logLevePrefixPadding = "        ";

        message = IndentLines(message, levelPrefix[level], logLevePrefixPadding);

        WriteLine(message);
    }

    public void LogDebug(string message)
    {
        Log(LoggingLevel.Debug, message);
    }

    public void LogDebug(Func<string> messageGenerator)
    {
        if (!IsLogging(LoggingLevel.Debug))
        {
            return;
        }

        Log(LoggingLevel.Debug, messageGenerator());
    }

    public void LogDebug(string message, params object[] messageArgs)
    {
        Log(LoggingLevel.Debug, string.Format(message, messageArgs));
    }

    public void LogError(string message)
    {
        HasError = true;
        ErrorMessages.Add(message);
        Log(LoggingLevel.Error, message);
    }

    public void LogError(string message, params object[] messageArgs)
    {
        LogError(string.Format(message, messageArgs));
    }

    public void LogError(Exception exception)
    {
        HasError = true;
        var message = $"Exception - {exception.Message}\nStack trace: {exception.StackTrace}";
        LogError(message);
    }

    public void LogError(DiagnosticCodeBase code)
    {
        LogError(code.MessageWithCode);
    }

    public void LogInfo(string message)
    {
        Log(LoggingLevel.Info, message);
    }

    public void LogInfo(string message, params object[] messageArgs)
    {
        if (Level < LoggingLevel.Info)
        {
            return;
        }

        LogInfo(string.Format(message, messageArgs));
    }

    public void LogTrace(string message)
    {
        Log(LoggingLevel.Trace, message);
    }

    public void LogTrace(Func<string> messageGenerator)
    {
        if (!IsLogging(LoggingLevel.Trace))
        {
            return;
        }

        Log(LoggingLevel.Trace, messageGenerator());
    }

    public void LogTrace(string message, params object[] messageArgs)
    {
        if (Level < LoggingLevel.Trace)
        {
            return;
        }

        LogTrace(string.Format(message, messageArgs));
    }

    public void LogWarning(string message)
    {
        Log(LoggingLevel.Warning, message);
    }

    public void LogWarning(string format, params object[] args)
    {
        if (Level < LoggingLevel.Warning)
        {
            return;
        }

        LogWarning(string.Format(format, args));
    }

    public void LogWarning(Exception exception)
    {
        if (Level < LoggingLevel.Warning)
        {
            return;
        }

        LogWarning($"Exception - {exception.Message}");
    }

    public void LogWarning(DiagnosticCodeBase code)
    {
        LogWarning(code.MessageWithCode);
    }

    protected abstract void WriteLine(string message);
}