using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using Semver;


namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

#pragma warning disable CS1591
/// <summary>
///     Information about a commit's versioning tags.
/// </summary>
public sealed class TagMetadata
{
    /// <summary>
    ///     Create a tag metadata for a commit that does not have any versioning tag.
    /// </summary>
    public TagMetadata()
        : this(ReleaseTypeId.NotReleased,
               null,
               new ApiChangeFlags())
    {
    }

    public TagMetadata(ReleaseTypeId state, ApiChangeFlags changeFlags)
        : this(state,
               null,
               changeFlags)
    {
    }

    public TagMetadata(ReleaseTypeId state, SemVersion version)
        : this(state, version, new ApiChangeFlags())
    {
    }

    /// <summary>
    ///     Information about a commit's release state.
    /// </summary>
    public TagMetadata(ReleaseTypeId state, SemVersion? version, ApiChangeFlags changeFlags)
    {
        Version = version;
        ReleaseType = state;
        ChangeFlags = changeFlags;

        if ((state == ReleaseTypeId.NotReleased && Version != null) ||
            (state == ReleaseTypeId.Released && Version == null))
        {
            throw new
                Git2SemVerInvalidOperationException($"Property {nameof(Version)} and the state {nameof(ReleaseTypeId.NotReleased)} are mutually exclusive.");
        }
    }

    /// <summary>
    ///     Changes (breaking, features, or fixes).
    /// </summary>
    /// <remarks>
    ///     If a release tag all change flags are ignored.
    /// </remarks>
    [JsonPropertyOrder(3)]
    public ApiChangeFlags ChangeFlags { get; }

    /// <summary>
    ///     Indicates if this tag is a release tag.
    /// </summary>
    [JsonIgnore]
    public bool IsARelease => ReleaseType == ReleaseTypeId.Released;

    /// <summary>
    ///     Indicates if this tag is a waypoint tag.
    /// </summary>
    [JsonIgnore]
    public bool IsAWaypoint => ReleaseType == ReleaseTypeId.ReleaseWaypoint;

    /// <summary>
    ///     Indicates if the owning commit is the root commit without a versioning tag.
    /// </summary>
    [JsonIgnore]
    public bool IsRootCommit => ReleaseType == ReleaseTypeId.RootCommit;

    /// <summary>
    ///     The versioning type. Indicates if a versioning tag was found, and if so, if a release tag or a waypoint tag.
    /// </summary>
    [JsonPropertyOrder(1)]
    public ReleaseTypeId ReleaseType { get; }

    /// <summary>
    ///     Version to be used in the context of the State (ReleaseStateId).
    /// </summary>
    /// <remarks>
    ///     If <c>State</c> is <c>Released</c>, the released version.
    ///     If <c>State</c> is <c>ReleaseWaypoint</c>, the prior released version.
    ///     If <c>State</c> is <c>RootCommit</c>, then null.
    ///     If <c>State</c> is <c>NoReleased</c>, then null.
    /// </remarks>
    [JsonPropertyOrder(2)]
    public SemVersion? Version { get; }
}