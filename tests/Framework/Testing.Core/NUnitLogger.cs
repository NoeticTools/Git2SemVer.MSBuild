using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Testing.Core;

/// <summary>
///     Logger to log to NUnit's TestContext.
/// </summary>
public class NUnitLogger : ILogger
{
    private const string LogScopeIndent = "  ";
    private readonly string _debugPrefix = "DEBUG: ";
    private readonly List<string> _errorMessages = [];
    private readonly string _errorPrefix = "ERROR: ";
    private readonly string _infoPrefix = "INFO: ";
    private readonly string _tracePrefix = "TRACE: ";
    private readonly string _warnPrefix = "WARN: ";

    public NUnitLogger(bool showMessageLevelPrefix = true)
    {
        if (showMessageLevelPrefix)
        {
            return;
        }

        _errorPrefix = "";
        _warnPrefix = "";
        _infoPrefix = "";
        _debugPrefix = "";
        _tracePrefix = "";
    }

    public string Errors => string.Join(Environment.NewLine, _errorMessages);

    public bool HasError { get; private set; }

    public LoggingLevel Level { get; set; } = LoggingLevel.Info;

    public string MessagePrefix { get; private set; } = "";

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public IDisposable EnterLogScope()
    {
        MessagePrefix += LogScopeIndent;
        return new UsingScope(() => { MessagePrefix = MessagePrefix.Substring(0, MessagePrefix.Length - LogScopeIndent.Length); });
    }

    public bool IsLogging(LoggingLevel level)
    {
        return Level >= level;
    }

    public void Log(LoggingLevel level, string message)
    {
        if (Level > level)
        {
            return;
        }

        var lookup = new Dictionary<LoggingLevel, Action<string>>
        {
            { LoggingLevel.Trace, LogTrace },
            { LoggingLevel.Debug, LogDebug },
            { LoggingLevel.Info, LogInfo },
            { LoggingLevel.Warning, LogWarning },
            { LoggingLevel.Error, LogError }
        };

        lookup[level](message);
    }

    public void LogDebug(string message)
    {
        if (Level >= LoggingLevel.Debug)
        {
            TestContext.Out.WriteLine(_debugPrefix + IndentLines(message));
        }
    }

    public void LogDebug(Func<string> messageGenerator)
    {
        if (Level < LoggingLevel.Debug)
        {
            return;
        }

        LogDebug(messageGenerator());
    }

    public void LogDebug(string message, params object[] messageArgs)
    {
        LogDebug(string.Format(message, messageArgs));
    }

    public void LogError(string message)
    {
        HasError = true;
        _errorMessages.Add(message);
        if (Level < LoggingLevel.Error)
        {
            return;
        }

        var logMessage = _errorPrefix + MessagePrefix + message;
        TestContext.Error.WriteLine(logMessage);
        Assert.Fail(logMessage);
    }

    public void LogError(string message, params object[] messageArgs)
    {
        LogError(string.Format(message, messageArgs));
    }

    public void LogError(Exception exception)
    {
        HasError = true;
        var message = $"Exception: {exception.Message}\nStack trace: {exception.StackTrace}";
        _errorMessages.Add(message);
        if (Level < LoggingLevel.Error)
        {
            return;
        }

        TestContext.Error.WriteLine(message);
        Assert.Fail(message);
    }

    public void LogError(DiagnosticCodeBase code)
    {
        LogError(code.MessageWithCode);
    }

    public void LogInfo(string message)
    {
        if (Level >= LoggingLevel.Info)
        {
            TestContext.Out.WriteLine(_infoPrefix + IndentLines(message));
        }
    }

    public void LogInfo(string message, params object[] messageArgs)
    {
        LogInfo(string.Format(message, messageArgs));
    }

    public void LogTrace(string message)
    {
        if (Level >= LoggingLevel.Trace)
        {
            TestContext.Out.WriteLine(_tracePrefix + IndentLines(message));
        }
    }

    public void LogTrace(Func<string> messageGenerator)
    {
        if (Level < LoggingLevel.Trace)
        {
            return;
        }

        LogTrace(messageGenerator());
    }

    public void LogTrace(string message, params object[] messageArgs)
    {
        LogTrace(string.Format(message, messageArgs));
    }

    public void LogWarning(DiagnosticCodeBase code)
    {
        LogWarning(code.MessageWithCode);
    }

    public void LogWarning(string message)
    {
        if (Level >= LoggingLevel.Warning)
        {
            TestContext.Out.WriteLine(_warnPrefix + IndentLines(message));
        }
    }

    public void LogWarning(string format, params object[] args)
    {
        LogWarning(string.Format(format, args));
    }

    public void LogWarning(Exception exception)
    {
        LogWarning($"Exception - {exception.Message}");
    }

    private string IndentLines(string message)
    {
        return IndentLines(message, MessagePrefix, MessagePrefix);
    }

    private string IndentLines(string message, string firstLinePrefix, string followingLinesPrefix)
    {
        var lines = Regex.Split(message, "\r\n|\r|\n");
        return firstLinePrefix + MessagePrefix + string.Join(Environment.NewLine + followingLinesPrefix + MessagePrefix, lines);
    }
}