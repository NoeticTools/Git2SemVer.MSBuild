namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public class ChangeGeneratorOptions(IChangeGeneratorOptions options) : IChangeGeneratorOptions
{
    public string ChangelogArtifactLinkPattern { get; set; } = options.ChangelogArtifactLinkPattern;

    public string ChangelogDataDirectory { get; set; } =
        ToAbsolutePath(options.ChangelogDataDirectory, ChangelogConstants.DefaultDataDirectory, options.WorkingDirectory);

    public bool ChangelogEnable { get; set; } = options.ChangelogEnable;

    public string ChangelogOutputFilePath { get; set; } =
        ToAbsolutePath(options.ChangelogOutputFilePath, ChangelogConstants.DefaultFilename, options.WorkingDirectory);

    public string ChangelogReleaseAs { get; set; } = options.ChangelogReleaseAs;

    private static string ToAbsolutePath(string path, string defaultPath, string workingDirectory)
    {
        if (path.Length == 0)
        {
            path = defaultPath;
        }

        if (defaultPath.Length > 0 && Path.IsPathRooted(path))
        {
            return path;
        }

        return Path.Combine(workingDirectory, path);
    }

    public string WorkingDirectory { get; } = options.WorkingDirectory;
}