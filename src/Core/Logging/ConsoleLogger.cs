using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.Diagnostics;
using AnsiConsole = Spectre.Console.AnsiConsole;


namespace NoeticTools.Git2SemVer.Core.Logging;

[RegisterTransient]
[ExcludeFromCodeCoverage]
public sealed class ConsoleLogger : LoggerBase, ILogger
{
    public override LoggingLevel Level { get; set; } = LoggingLevel.Info;

    public void Dispose()
    {
    }

    public void Log(LoggingLevel level, string message)
    {
        if (!IsLogging(level))
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
        if (!IsLogging(LoggingLevel.Debug))
        {
            return;
        }

        message = IndentLines(message);
        System.Console.Out.WriteLine(message);
    }

    public void LogDebug(Func<string> messageGenerator)
    {
        if (!IsLogging(LoggingLevel.Debug))
        {
            return;
        }

        LogDebug(messageGenerator());
    }

    public void LogDebug(string message, params object[] messageArgs)
    {
        if (!IsLogging(LoggingLevel.Debug))
        {
            return;
        }

        LogDebug(string.Format(message, messageArgs));
    }

    public void LogError(string message)
    {
        HasError = true;
        ErrorMessages.Add(message);
        AnsiConsole.MarkupLine("[red]" + message + "[/]");
    }

    public void LogError(string message, params object[] messageArgs)
    {
        LogError(MessagePrefix + string.Format(message, messageArgs));
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
        if (!IsLogging(LoggingLevel.Info))
        {
            return;
        }

        message = IndentLines(message);
        System.Console.Out.WriteLine(message);
    }

    public void LogInfo(string message, params object[] messageArgs)
    {
        if (!IsLogging(LoggingLevel.Info))
        {
            return;
        }

        LogInfo(string.Format(message, messageArgs));
    }

    public void LogTrace(string message)
    {
        if (!IsLogging(LoggingLevel.Trace))
        {
            return;
        }

        message = IndentLines(message);
        AnsiConsole.MarkupLine("[grey50]" + message + "[/]");
    }

    public void LogTrace(Func<string> messageGenerator)
    {
        if (!IsLogging(LoggingLevel.Trace))
        {
            return;
        }

        LogTrace(messageGenerator());
    }

    public void LogTrace(string message, params object[] messageArgs)
    {
        if (!IsLogging(LoggingLevel.Trace))
        {
            return;
        }

        LogTrace(string.Format(message, messageArgs));
    }

    public void LogWarning(string message)
    {
        if (!IsLogging(LoggingLevel.Warning))
        {
            return;
        }

        message = IndentLines(message);
        AnsiConsole.MarkupLine("[fuchsia]" + message + "[/]");
    }

    public void LogWarning(string format, params object[] args)
    {
        if (!IsLogging(LoggingLevel.Warning))
        {
            return;
        }

        LogWarning(string.Format(format, args));
    }

    public void LogWarning(Exception exception)
    {
        if (!IsLogging(LoggingLevel.Warning))
        {
            return;
        }

        LogWarning($"Exception - {exception.Message}");
    }

    public void LogWarning(DiagnosticCodeBase code)
    {
        LogWarning(code.MessageWithCode);
    }
}