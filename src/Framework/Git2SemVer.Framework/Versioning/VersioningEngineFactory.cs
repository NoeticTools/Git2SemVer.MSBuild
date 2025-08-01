using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;
using NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;


namespace NoeticTools.Git2SemVer.Framework.Versioning;

[ExcludeFromCodeCoverage]
[RegisterSingleton]
public sealed class VersioningEngineFactory(ILogger logger)
{
    public IVersioningEngine Create(IVersionGeneratorInputs inputs,
                                    IMSBuildGlobalProperties msBuildGlobalProperties,
                                    IOutputsJsonIO outputsJsonIO,
                                    IBuildHost host,
                                    ConventionalCommitsSettings convCommitsSettings)
    {
        var gitTool = new GitTool(new TagParser(inputs.ReleaseTagFormat),
                                  new ConventionalCommitsParser(convCommitsSettings))
        {
            RepositoryDirectory = inputs.WorkingDirectory
        };
        var gitPathsFinder = new GitHistoryWalker(gitTool, logger);

        var defaultBuilderFactory = new DefaultVersionBuilderFactory(logger);
        var scriptBuilder = new ScriptVersionBuilder(logger);
        var versionGenerator = new VersioningEngine(inputs,
                                                    host,
                                                    outputsJsonIO,
                                                    gitTool,
                                                    gitPathsFinder,
                                                    defaultBuilderFactory,
                                                    scriptBuilder,
                                                    msBuildGlobalProperties,
                                                    logger);
        return versionGenerator;
    }
}