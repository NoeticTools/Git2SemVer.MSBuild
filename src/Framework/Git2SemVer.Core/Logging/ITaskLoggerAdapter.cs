using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Logging;

/// <summary>
///     An internally used interface.
/// </summary>
public interface ITaskLoggerAdapter
{
    void LogDebug(string message);
    void LogError(string message);
    void LogError(Exception exception);
    void LogError(DiagnosticCodeBase code);
    void LogInfo(string message);
    void LogTrace(string message);
    void LogWarning(string message);
    void LogWarning(Exception exception);
    void LogWarning(DiagnosticCodeBase exception);
}