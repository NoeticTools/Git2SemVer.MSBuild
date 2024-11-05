using NoeticTools.Common.Tools.Git;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.TestScenarios;

public sealed class GitTestRepository
{
    public GitTestRepository(string description,
                             Commit[] commits,
                             string headCommitId,
                             int expectedPathCount,
                             string expectedVersion)
    {
        Description = description;
        Commits = commits;
        HeadCommitId = headCommitId;
        ExpectedPathCount = expectedPathCount;
        ExpectedVersion = expectedVersion;
    }

    public Commit[] Commits { get; }

    public string Description { get; }

    public int ExpectedPathCount { get; }

    public string ExpectedVersion { get; }

    public string HeadCommitId { get; }
}