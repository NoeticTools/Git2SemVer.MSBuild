

// ReSharper disable UnusedMember.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class ChangelogScribanModel(
    ChangelogInputs inputs,
    IReadOnlyList<ChangeCategory> categories,
    string releaseUrl,
    bool incremental)
{
    /// <summary>
    ///     The git branch that the head commit is on.
    /// </summary>
    public string BranchName { get; } = inputs.BranchName;

    public IReadOnlyList<ChangeCategory> Categories { get; } = categories;

    public DateTime HeadDateTime { get; } = inputs.HeadCommitWhen.DateTime;

    public string HeadSha { get; } = inputs.HeadCommitSha;

    public bool Incremental { get; } = incremental;

    public bool IsPrerelease { get; } = inputs.Version!.IsPrerelease;

    public bool IsRelease { get; } = inputs.Version.IsRelease;

    public int NumberOfCommits { get; } = inputs.ConventionalCommits.Count;

    public DateTime ReleaseDate { get; } = DateTime.Now;

    public string ReleaseUrl { get; } = releaseUrl;

    public string SemVersion { get; } = inputs.Version.ToString();
}