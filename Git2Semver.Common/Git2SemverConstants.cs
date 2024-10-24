using System.ComponentModel;


namespace NoeticTools.Git2Semver.Common;

public static class Git2SemverConstants
{
    public const string DefaultScriptFilename = "Git2SemVer.csx";

    public const string SharedVersionPropertiesFilename = "Git2SemVer.Generated.Properties.g.props";

    public const string SharedVersionJsonPropertiesFilename = "Git2SemVer.Generated.Properties.g.json";

    /// <summary>
    /// This folder name is hard coded in Directory.Versioning.Build.props.
    /// Do not change as it will be a breaking change.
    /// </summary>
    public const string ShareFolderName = ".git2semver";
}