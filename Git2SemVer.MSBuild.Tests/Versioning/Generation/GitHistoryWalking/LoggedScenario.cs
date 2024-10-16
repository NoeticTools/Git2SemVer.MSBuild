namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal sealed class LoggedScenario
{
    public string ExpectedVersion { get; }

    public string ActualGitLog { get; }

    public string HeadCommitId { get; }

    public LoggedScenario(string expectedVersion, string headCommitId, string actualGitLog)
    {
        ExpectedVersion = expectedVersion;
        ActualGitLog = actualGitLog;
        HeadCommitId = headCommitId;
    }
}