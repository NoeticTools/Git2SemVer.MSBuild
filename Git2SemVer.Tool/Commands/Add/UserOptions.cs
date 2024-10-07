namespace NoeticTools.Git2SemVer.Tool.Commands.Add;

internal struct UserOptions
{
    public string VersioningProjectName { get; }

    public string VersionTagPrefix { get; }

    public UserOptions(string leadingProjectName, string versionTagPrefix)
    {
        VersioningProjectName = leadingProjectName;
        VersionTagPrefix = versionTagPrefix;
    }
}