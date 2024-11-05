namespace NoeticTools.Git2SemVer.MSBuild.Framework.Config;
// ReSharper disable UnusedAutoPropertyAccessor.Global

internal sealed class Git2SemVerBuildLogEntry
{
    public Git2SemVerBuildLogEntry(string buildNumber, bool hasLocalChanges, string branch, string lastCommitId, string path)
    {
        BuildNumber = buildNumber;
        HasLocalChanges = hasLocalChanges;
        Branch = branch;
        LastCommitId = lastCommitId;
        Path = path.Replace('\\', '/');
        Timestamp = DateTime.Now.ToString("s");
    }

    public string Branch { get; }

    public string BuildNumber { get; }

    /// <summary>
    ///     If true then there were local, not commited, changes at time of build.
    /// </summary>
    public bool HasLocalChanges { get; }

    /// <summary>
    ///     Last commit ID at time of build.
    /// </summary>
    public string LastCommitId { get; }

    /// <summary>
    ///     The directory that was used for the git working directory.
    ///     This is usually the solution directory.
    /// </summary>
    public string Path { get; }

    public string Timestamp { get; }
}