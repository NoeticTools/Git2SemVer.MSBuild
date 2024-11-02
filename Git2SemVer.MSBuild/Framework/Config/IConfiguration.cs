using NoeticTools.Git2SemVer.MSBuild.Tools.CI;


namespace NoeticTools.Git2SemVer.MSBuild.Framework.Config;

internal interface IConfiguration
{
    List<Git2SemVerBuildLogEntry> BuildLog { get; set; }

    /// <summary>
    ///     The local build log size.
    ///     If zero, the local build log is cleared and disabled.
    /// </summary>
    int BuildLogSizeLimit { get; set; }

    /// <summary>
    ///     The next local build number. Default is 1.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <b>Not recommended for use on build system (controlled) build hosts.</b>
    ///     </para>
    ///     <para>
    ///         Used by <a cref="UncontrolledHost">UncontrolledHost.BuildNumber</a>/
    ///     </para>
    /// </remarks>
    int BuildNumber { get; set; }

    /// <summary>
    ///     This configuration's schema revision count. To facilitate future migration.
    /// </summary>
    int Rev { get; set; }

    Git2SemVerBuildLogEntry AddLogEntry(string buildNumber, bool hasLocalChanges, string branch, string lastCommitId, string path);

    /// <summary>
    ///     Save configuration to file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Saves the user's Git2SemVer configuration file.
    ///     </para>
    /// </remarks>
    void Save();
}