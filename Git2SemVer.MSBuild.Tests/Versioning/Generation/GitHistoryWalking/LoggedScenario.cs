namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal sealed class LoggedScenario
{
    public string ExpectedVersion { get; }

    public string ActualGitLog { get; }

    public LoggedScenario(string expectedVersion, string actualGitLog)
    {
        ExpectedVersion = expectedVersion;
        ActualGitLog = actualGitLog;
    }
}