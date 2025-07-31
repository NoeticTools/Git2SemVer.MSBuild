using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.MSBuild.Tasks;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public sealed class ChangelogGeneratorTask
{
    private readonly IChangelogTaskOptions _options;
    private readonly IVersionOutputs _versionOutputs;
    private readonly ILogger _logger;

    public ChangelogGeneratorTask(IChangelogTaskOptions options, IVersionOutputs versionOutputs, ILogger logger)
    {
        _options = options;
        _versionOutputs = versionOutputs;
        _logger = logger;
    }

    public void Execute()
    {
        var projectSettings = ChangelogProjectSettings.Load(_options.ChangelogDataDirectory, ChangelogConstants.ProjectSettingsFilename);

    }
}