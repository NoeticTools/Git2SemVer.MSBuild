namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;

internal struct UserOptions
{
    public string VersioningProjectName { get; }

    public string VersionTagPrefix { get; } // todo

    public UserOptions(string leadingProjectName, string versionTagPrefix)
    {
        VersioningProjectName = leadingProjectName;
        VersionTagPrefix = versionTagPrefix;
    }
}