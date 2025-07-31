using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Tests.TestScenarios;

public class GitTestRepository(
    string description,
    Commit[] commits,
    string headCommitId,
    int expectedPathCount,
    string expectedVersion)
{
    public Commit[] Commits { get; protected set; } = commits;

    public string Description { get; } = description;

    public int ExpectedPathCount { get; protected set; } = expectedPathCount;

    public string ExpectedVersion { get; } = expectedVersion;

    public string HeadCommitId { get; protected set; } = headCommitId;
}