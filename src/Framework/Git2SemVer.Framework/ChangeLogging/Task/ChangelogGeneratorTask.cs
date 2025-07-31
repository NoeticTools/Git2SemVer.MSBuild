using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.MSBuild.Tasks;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public sealed class ChangelogGeneratorTask(IChangelogTaskOptions options, ILogger logger)
{
    public void Execute(IVersionOutputs versionOutputs, SemanticVersionCalcResult? calcData)
    {
        if (calcData == null)
        {
            return;
        }
        var projectSettings = ChangelogProjectSettings.Load(options.ChangelogDataDirectory, ChangelogConstants.ProjectSettingsFilename);
        new ChangelogGenerator(projectSettings, logger).Execute(new ConventionalCommitsVersionInfo(versionOutputs, calcData.Contributing),
                                                                options.ChangelogArtifactLinkPattern,
                                                                options.ChangelogReleaseAs,
                                                                options.ChangelogDataDirectory,
                                                                options.ChangelogOutputFilePath);
    }
}