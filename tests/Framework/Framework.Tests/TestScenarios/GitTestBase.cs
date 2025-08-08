using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Tests.TestScenarios;

public class GitTestBase
{
    protected static Commit NewCommit(string sha, string[] parents, string summary, string refs = "", ICommitMessageMetadata? metadata = null)
    {
        return new Commit(sha, parents, summary, refs, metadata ?? CommitMessageMetadata.Null);
    }
}