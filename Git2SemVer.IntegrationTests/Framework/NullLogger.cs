using NoeticTools.Git2SemVer.Core.Logging;


namespace Git2SemVer.IntTests.Framework;

internal class NullLogger : ILogger
{
    public void Dispose()
    {
    }

    public IDisposable EnterLogScope()
    {
        return this;
    }

    public void Log(LoggingLevel level, string message)
    {
    }

    public void LogDebug(string message)
    {
    }

    public void LogDebug(Func<string> messageGenerator)
    {
    }

    public void LogDebug(string message, params object[] messageArgs)
    {
    }

    public void LogError(string message)
    {
        throw new NotImplementedException();
    }

    public void LogError(string message, params object[] messageArgs)
    {
    }

    public void LogError(Exception exception)
    {
    }

    public void LogInfo(string message)
    {
    }

    public void LogInfo(string message, params object[] messageArgs)
    {
    }

    public void LogTrace(string message)
    {
    }

    public void LogTrace(Func<string> messageGenerator)
    {
    }

    public void LogTrace(string message, params object[] messageArgs)
    {
    }

    public void LogWarning(string message)
    {
    }

    public void LogWarning(string format, params object[] args)
    {
    }

    public void LogWarning(Exception exception)
    {
    }

    public string Errors => "";

    public bool HasError => false;

    public LoggingLevel Level { get; set; }

    public string LogPrefix => "";
}