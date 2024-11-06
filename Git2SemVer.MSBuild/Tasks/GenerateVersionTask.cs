using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.Config;
using NoeticTools.Git2SemVer.MSBuild.Tools.CI;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;


namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

public sealed class GenerateVersionTask
{
    private readonly ILogger _logger;

    public GenerateVersionTask(ILogger logger)
    {
        _logger = logger;
    }

    public IVersionOutputs GenerateVersions(IVersionGeneratorInputs inputs)
    {
        var config = Git2SemVerConfiguration.Load();
        var host = new BuildHostFactory(config, _logger).Create(inputs.HostType,
                                                               inputs.BuildNumber,
                                                               inputs.BuildContext,
                                                               inputs.BuildIdFormat);
        var gitTool = new GitTool(_logger)
        {
            WorkingDirectory = inputs.WorkingDirectory
        };
        var commitsRepo = new CommitsRepository(gitTool);
        var gitPathsFinder = new PathsFromLastReleasesFinder(commitsRepo, gitTool, _logger);

        var defaultBuilderFactory = new DefaultVersionBuilderFactory(_logger);
        var scriptBuilder = new ScriptVersionBuilder(_logger);
        var versionGenerator = new VersionGenerator(inputs, host,
                                                    new GeneratedVersionsJsonFile(),
                                                    new GeneratedVersionsPropsFile(),
                                                    gitTool, gitPathsFinder, defaultBuilderFactory,
                                                    scriptBuilder, _logger);
        return versionGenerator.Run();
    }
}