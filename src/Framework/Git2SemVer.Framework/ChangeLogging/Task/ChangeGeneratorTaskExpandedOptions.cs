namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public class ChangeGeneratorTaskExpandedOptions(IChangeLogGeneratorTaskOptions taskOptions) : IChangeLogGeneratorTaskOptions
{
    public string ChangelogArtifactLinkPattern { get; set; } = taskOptions.ChangelogArtifactLinkPattern;

    public string ChangelogDataDirectory { get; set; } =
        ToAbsolutePath(taskOptions.ChangelogDataDirectory, ChangelogConstants.DefaultDataDirectory, taskOptions.WorkingDirectory);

    public bool ChangelogEnable { get; set; } = taskOptions.ChangelogEnable;

    public string ChangelogOutputFilePath { get; set; } =
        ToAbsolutePath(taskOptions.ChangelogOutputFilePath, ChangelogConstants.DefaultFilename, taskOptions.WorkingDirectory);

    public string ChangelogReleaseAs { get; set; } = taskOptions.ChangelogReleaseAs;

    public string WorkingDirectory { get; } = taskOptions.WorkingDirectory;

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