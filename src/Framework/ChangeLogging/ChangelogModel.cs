using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Semver;


// ReSharper disable UnusedMember.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class ChangelogModel(
    SemVersion version,
    ContributingCommits contributing,
    IReadOnlyList<ChangeCategory> categories,
    string releaseUrl,
    bool incremental)
{
    /// <summary>
    ///     The git branch that the head commit is on.
    /// </summary>
    public string BranchName { get; } = contributing.BranchName;

    public IReadOnlyList<ChangeCategory> Categories { get; } = categories;

    public DateTime HeadDateTime { get; } = contributing.Head.When.DateTime;

    public string HeadSha { get; } = contributing.Head.CommitId.Sha;

    public bool Incremental { get; } = incremental;

    public bool IsPrerelease { get; } = version.IsPrerelease;

    public bool IsRelease { get; } = version.IsRelease;

    public int NumberOfCommits { get; } = contributing.Commits.Count;

    public DateTime ReleaseDate { get; } = DateTime.Now;

    public string ReleaseUrl { get; } = releaseUrl;

    public string SemVersion { get; } = version.ToString();
}