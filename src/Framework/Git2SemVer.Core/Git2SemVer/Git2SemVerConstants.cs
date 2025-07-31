namespace NoeticTools.Git2SemVer.Core.Git2SemVer;

public static class Git2SemVerConstants
{
    public const string DefaultScriptFilename = "Git2SemVer.csx";

    public const string SharedVersionJsonPropertiesFilename = "Git2SemVer.VersionInfo.g.json";

    public const string SharedVersionPropertiesFilename = "Git2SemVer.VersionInfo.g.props";

    /// <summary>
    ///     This folder name is hard coded in Directory.Versioning.Build.props.
    ///     Do not change as it will be a breaking change.
    /// </summary>
    public const string ShareFolderName = ".git2semver";
}