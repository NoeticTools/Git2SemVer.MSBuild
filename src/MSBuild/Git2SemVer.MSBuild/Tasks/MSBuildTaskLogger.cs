using System.Diagnostics.CodeAnalysis;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Core.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

[ExcludeFromCodeCoverage]
public class MSBuildTaskLogger : TaskLoggerBase
{
    public MSBuildTaskLogger(TaskLoggingHelper inner) :
        base(new MSBuildLoggerAdapter(inner))
    {
    }

    private class MSBuildLoggerAdapter : ITaskLoggerAdapter
    {
        private readonly TaskLoggingHelper _inner;

        public MSBuildLoggerAdapter(TaskLoggingHelper inner)
        {
            _inner = inner;
        }

        public void LogDebug(string message)
        {
            _inner.LogMessage(MessageImportance.Normal, message);
        }

        public void LogError(string message)
        {
            _inner.LogError(message);
        }

        public void LogError(Exception exception)
        {
            _inner.LogErrorFromException(exception, true, true, null);
        }

        public void LogError(DiagnosticCodeBase code)
        {
            _inner.LogError(code.SubCategory,
                            code.Code,
                            null,
                            code.HelpLink,
                            null,
                            0,
                            0,
                            0,
                            0,
                            code.Message);
        }

        public void LogInfo(string message)
        {
            _inner.LogMessage(MessageImportance.High, message);
        }

        public void LogTrace(string message)
        {
            _inner.LogMessage(MessageImportance.Low, message);
        }

        public void LogWarning(string message)
        {
            _inner.LogWarning(message);
        }

        public void LogWarning(Exception exception)
        {
            _inner.LogWarningFromException(exception);
        }

        public void LogWarning(DiagnosticCodeBase code)
        {
            _inner.LogWarning(code.SubCategory,
                              code.Code,
                              null,
                              code.HelpLink,
                              null,
                              0,
                              0,
                              0,
                              0,
                              code.Message);
        }
    }
}