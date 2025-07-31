namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public interface IChangeGeneratorOptions
{
    /// <summary>
    ///     Optional changelog url to a version's artifacts. May contain version placeholder '%VERSION%'.
    /// </summary>
    string ChangelogArtifactLinkPattern { get; set; }

    /// <summary>
    ///     Path to changelog generator's data and configuration files directory. May be a relative or absolute path.
    /// </summary>
    string ChangelogDataDirectory { get; set; } // todo - constant

    /// <summary>
    ///     Optional changelog generation/update enable.
    /// </summary>
    bool ChangelogEnable { get; set; }

    /// <summary>
    ///     Generated changelog file path. May be a relative or absolute path. Set to empty string to disable file write.
    /// </summary>
    string ChangelogOutputFilePath { get; set; }

    /// <summary>
    ///     If not an empty string, sets the changelog's changes version (normally version or 'Unreleased'). Any text
    ///     permitted.
    /// </summary>
    string ChangelogReleaseAs { get; set; }
}