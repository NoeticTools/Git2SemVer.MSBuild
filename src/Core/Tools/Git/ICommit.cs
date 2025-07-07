using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;
using Semver;


namespace NoeticTools.Git2SemVer.Core.Tools.Git;

[JsonDerivedType(typeof(Commit), "Commit")]
public interface ICommit
{
    /// <summary>
    ///     Commit ID.
    /// </summary>
    CommitId CommitId { get; }

    /// <summary>
    ///     Commit message body or description.
    /// </summary>
    string MessageBody { get; }

    /// <summary>
    ///     The commit's parent commits.
    /// </summary>
    CommitId[] Parents { get; }

    /// <summary>
    ///     The released version parsed from an associated release tag or null if no release tag found.
    /// </summary>
    [Obsolete("Depreciated and will be removed in a future release. Use ReleaseState property instead.")]
    SemVersion? ReleasedVersion { get; }

    /// <summary>
    ///     Commit message summary or title.
    /// </summary>
    string Summary { get; }

    /// <summary>
    ///     Release metadata obtained from the commit tags.
    /// </summary>
    TagMetadata TagMetadata { get; }

    /// <summary>
    ///     Tags associated with this commit.
    /// </summary>
    IReadOnlyList<IGitTag> Tags { get; }

    /// <summary>
    ///     When the commit was commited.
    /// </summary>
    DateTimeOffset When { get; }
}