

// ReSharper disable UnusedMember.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class ChangelogScribanModel(
    ConventionalCommitsVersionInfo inputs,
    IReadOnlyList<ChangeCategory> categories,
    string releaseUrl,
    bool incremental)
{
    /// <summary>
    /// Categories to group changes into.
    /// </summary>
    public IReadOnlyList<ChangeCategory> Categories { get; } = categories;

    /// <summary>
    /// True if generating an incremental changelog. Otherwise, generating a new non-incremental changelog.
    /// </summary>
    public bool Incremental { get; } = incremental;

    /// <summary>
    /// True if generating a changelog for a prerelease version.
    /// </summary>
    public bool IsPrerelease { get; } = inputs.Version!.IsPrerelease;

    /// <summary>
    /// True if generating a changelog for a release version.
    /// </summary>
    public bool IsRelease { get; } = inputs.Version.IsRelease;

    /// <summary>
    /// Date that will be shown on a release changelog.
    /// </summary>
    public DateTime ReleaseDate { get; } = DateTime.Now;

    /// <summary>
    /// Format string to build URL to artifact.
    /// </summary>
    public string ReleaseUrl { get; } = releaseUrl;

    /// <summary>
    /// Current version.
    /// </summary>
    public string SemVersion { get; } = inputs.Version.ToString();
}