namespace NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;

/// <summary>
///     Supported host types.
/// </summary>
public enum HostTypeIds
{
    /// <summary>
    ///     Host type is unknown. Used internally.
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     A custom host. Host properties are set by MSBuild properties.
    /// </summary>
    Custom = 1,

    /// <summary>
    ///     An uncontrolled host such as a developer's box.
    /// </summary>
    Uncontrolled = 2,

    /// <summary>
    ///     A TeamCity build host (agent).
    /// </summary>
    TeamCity = 3,

    /// <summary>
    ///     GitHub build host.
    /// </summary>
    GitHub = 4

    //Jenkins = 5,
}