using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Logging;

/// <summary>
///     MSBuild task logger. A wrapper for
///     [TaskLoggingHelper](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.utilities.tasklogginghelper?view=msbuild-17-netcore).
/// </summary>
/// <remarks>
///     <para>
///         Logs to the MSBuild output.
///         Logged errors will fail the build.
///     </para>
/// </remarks>
public interface ILogger : IDisposable
{
    /// <summary>
    ///     Errors that have logged by this logger.
    /// </summary>
    string Errors { get; }

    /// <summary>
    ///     True if this logger has logged an error.
    /// </summary>
    bool HasError { get; }

    LoggingLevel Level { get; set; }

    string MessagePrefix { get; }

    /// <summary>
    ///     A helper to indent messages within a scope.
    /// </summary>
    /// <returns></returns>
    IDisposable EnterLogScope();

    bool IsLogging(LoggingLevel level);

    void Log(LoggingLevel level, string message);

    /// <summary>
    ///     Log a message with [normal
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Wraps
    ///         [TaskLoggingHelper.LogDebug](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.utilities.tasklogginghelper.logmessage?view=msbuild-17-netcore)
    ///     </para>
    /// </remarks>
    void LogDebug(string message);

    void LogDebug(Func<string> messageGenerator);

    /// <summary>
    ///     Log a message with [normal
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Wraps
    ///         [TaskLoggingHelper.LogDebug](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.utilities.tasklogginghelper.logmessage?view=msbuild-17-netcore#microsoft-build-utilities-tasklogginghelper-logmessage(system-string-system-object()))
    ///     </para>
    /// </remarks>
    void LogDebug(string message, params object[] messageArgs);

    /// <summary>
    ///     Log an error.
    /// </summary>
    void LogError(string message);

    /// <summary>
    ///     Log an error.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    void LogError(string message, params object[] messageArgs);

    /// <summary>
    ///     Log an exception as an error.
    /// </summary>
    void LogError(Exception exception);

    void LogError(DiagnosticCodeBase code);

    /// <summary>
    ///     Log a message with [high
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    void LogInfo(string message);

    /// <summary>
    ///     Log a message with [high
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    void LogInfo(string message, params object[] messageArgs);

    /// <summary>
    ///     Log a message of [low
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    void LogTrace(string message);

    void LogTrace(Func<string> messageGenerator);

    /// <summary>
    ///     Log a message of [low
    ///     importance](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.messageimportance?view=msbuild-17-netcore).
    /// </summary>
    void LogTrace(string message, params object[] messageArgs);

    /// <summary>
    ///     Log a warning message.
    /// </summary>
    void LogWarning(string message);

    /// <summary>
    ///     Log a warning message.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    void LogWarning(string format, params object[] args);

    /// <summary>
    ///     Log an exception as a warning.
    /// </summary>
    void LogWarning(Exception exception);

    void LogWarning(DiagnosticCodeBase code);
}