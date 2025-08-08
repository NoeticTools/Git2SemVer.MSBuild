using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Tests.TestScenarios;

public class GitTestRepository(
    string description,
    Commit[] commits,
    string headCommitId,
    string expectedVersion) : GitTestBase
{
    public Commit[] Commits { get; protected init; } = commits;

    public string Description { get; } = description;

    public string ExpectedVersion { get; } = expectedVersion;

    public string HeadCommitId { get; protected init; } = headCommitId;
}