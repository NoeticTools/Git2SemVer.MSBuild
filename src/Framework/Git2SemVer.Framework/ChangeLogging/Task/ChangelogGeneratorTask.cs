using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging.Task;

[RegisterTransient]
public sealed class ChangelogGeneratorTask(IChangeGeneratorOptions options, ILogger logger)
{
    public void Execute(VersioningOutputs versioningOutput)
    {
        Git2SemVerArgumentException.ThrowIfNull(versioningOutput, nameof(versioningOutput));

        if (!options.ChangelogEnable || !versioningOutput.Metadata.CalculationPerformed)
        {
            return;
        }

        logger.LogInfo("Generating changelog.");

        var projectSettings = ChangelogProjectSettings.Load(options.ChangelogDataDirectory, ChangelogConstants.ProjectSettingsFilename);
        new ChangelogGenerator(projectSettings, logger).Execute(versioningOutput,
                                                                options.ChangelogArtifactLinkPattern,
                                                                options.ChangelogReleaseAs,
                                                                options.ChangelogDataDirectory,
                                                                options.ChangelogOutputFilePath);
    }
}