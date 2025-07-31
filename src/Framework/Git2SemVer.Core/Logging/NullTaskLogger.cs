using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Logging;

[ExcludeFromCodeCoverage]
public sealed class NullTaskLogger : TaskLoggerBase
{
    public NullTaskLogger() :
        base(new NullTaskLoggerAdapter())
    {
    }

    private sealed class NullTaskLoggerAdapter : ITaskLoggerAdapter
    {
        public void LogDebug(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogError(Exception exception)
        {
        }

        public void LogError(DiagnosticCodeBase code)
        {
        }

        public void LogInfo(string message)
        {
        }

        public void LogTrace(string message)
        {
        }

        public void LogWarning(string message)
        {
        }

        public void LogWarning(Exception exception)
        {
        }

        public void LogWarning(DiagnosticCodeBase exception)
        {
        }
    }
}