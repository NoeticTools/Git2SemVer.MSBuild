namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

#pragma warning disable CS1591
/// <summary>
///     A commit's release type.
/// </summary>
public enum ReleaseTypeId
{
    /// <summary>
    ///     Commit not released.
    /// </summary>
    NotReleased = 0,

    /// <summary>
    ///     Repository root commit, versioning starts at '0.1.0'.
    /// </summary>
    RootCommit = 1,

    /// <summary>
    ///     Released commit.
    /// </summary>
    Released = 2,

    /// <summary>
    ///     Waypoint commit representing versioning waypoint up to this commit.
    /// </summary>
    ReleaseWaypoint = 3
}