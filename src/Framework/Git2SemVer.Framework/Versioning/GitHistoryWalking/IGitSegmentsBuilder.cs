using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;

internal interface IGitSegmentsBuilder
{
    /// <summary>
    ///     Get all commits from releases reachable from the given commit as git segments.
    /// </summary>
    ContributingCommits GetContributingCommits(Commit headCommit);
}