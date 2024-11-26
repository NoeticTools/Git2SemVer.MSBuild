using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;
using NoeticTools.Git2SemVer.MSBuild.Tools.CI;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

internal sealed class ProjectVersioningFactory
{
    private readonly ILogger _logger;

    public ProjectVersioningFactory(ILogger logger)
    {
        _logger = logger;
    }

    public ProjectVersioning Create(IVersionGeneratorInputs inputs)
    {
        if (inputs == null)
        {
            throw new ArgumentNullException(nameof(inputs), "Inputs is required.");
        }

        var config = Git2SemVerConfiguration.Load();
        var host = new BuildHostFactory(config, _logger).Create(inputs.HostType,
                                                                inputs.BuildNumber,
                                                                inputs.BuildContext,
                                                                inputs.BuildIdFormat);
        var commitsRepo = new CommitsRepository();
        var gitTool = new GitTool(commitsRepo, _logger)
        {
            WorkingDirectory = inputs.WorkingDirectory
        };
        var gitPathsFinder = new PathsFromLastReleasesFinder(commitsRepo, gitTool, _logger);

        var defaultBuilderFactory = new DefaultVersionBuilderFactory(_logger);
        var scriptBuilder = new ScriptVersionBuilder(_logger);
        var generatedOutputsJsonFile = new GeneratedVersionsJsonFile();
        var versionGenerator = new VersionGenerator(inputs, host, generatedOutputsJsonFile, gitTool, gitPathsFinder, defaultBuilderFactory,
                                                    scriptBuilder, _logger);
        var projectVersioning = new ProjectVersioning(inputs, host,
                                                      generatedOutputsJsonFile, 
                                                      versionGenerator,
                                                      _logger);
        return projectVersioning;
    }
}