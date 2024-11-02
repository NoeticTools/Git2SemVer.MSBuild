namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal sealed class LoggedScenario
{
    public LoggedScenario(string expectedVersion, string headCommitId, string actualGitLog)
    {
        ExpectedVersion = expectedVersion;
        ActualGitLog = actualGitLog;
        HeadCommitId = headCommitId;
    }

    public string ActualGitLog { get; }

    public string ExpectedVersion { get; }

    public string HeadCommitId { get; }
}