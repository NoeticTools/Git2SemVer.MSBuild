using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.MSBuild.Tasks;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

public sealed class ChangelogGeneratorTask(IChangelogTaskOptions options, ILogger logger)
{
    public void Execute(IVersionOutputs versionOutputs, SemanticVersionCalcResult? calcData)
    {
        Git2SemVerArgumentException.ThrowIfNull(versionOutputs, nameof(versionOutputs));

        if (!options.ChangelogEnable || calcData == null)
        {
            return;
        }

        var projectSettings = ChangelogProjectSettings.Load(options.ChangelogDataDirectory, ChangelogConstants.ProjectSettingsFilename);
        new ChangelogGenerator(projectSettings, logger).Execute((versionOutputs, calcData),
                                                                options.ChangelogArtifactLinkPattern,
                                                                options.ChangelogReleaseAs,
                                                                options.ChangelogDataDirectory,
                                                                options.ChangelogOutputFilePath);
    }
}