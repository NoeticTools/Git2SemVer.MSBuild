using NoeticTools.Git2SemVer.Framework.Tools.CI;


namespace NoeticTools.Git2SemVer.Framework.Framework.Config;

public interface IConfiguration
{
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
    // ReSharper disable once UnusedMemberInSuper.Global
    int Rev { get; set; }

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