namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public class ChangeGeneratorOptions(IChangeGeneratorOptions options, string workingDirectory) : IChangeGeneratorOptions
{
    public string ChangelogArtifactLinkPattern { get; set; } = options.ChangelogArtifactLinkPattern;

    public string ChangelogDataDirectory { get; set; } =
        ToAbsolutePath(options.ChangelogDataDirectory, ChangelogConstants.DefaultDataDirectory, workingDirectory);

    public bool ChangelogEnable { get; set; } = options.ChangelogEnable;

    public string ChangelogOutputFilePath { get; set; } =
        ToAbsolutePath(options.ChangelogOutputFilePath, ChangelogConstants.DefaultFilename, workingDirectory);

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
}