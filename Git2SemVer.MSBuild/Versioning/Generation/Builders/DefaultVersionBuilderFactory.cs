using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;

internal sealed class DefaultVersionBuilderFactory : IDefaultVersionBuilderFactory
{
    private readonly ILogger _logger;

    public DefaultVersionBuilderFactory(ILogger logger)
    {
        _logger = logger;
    }

    public IVersionBuilder Create(HistoryPaths historyPaths, IBuildHost host, IVersionGeneratorInputs inputs, IVersionOutputs outputs)
    {
        return new DefaultVersionBuilder(historyPaths, _logger);
    }
}