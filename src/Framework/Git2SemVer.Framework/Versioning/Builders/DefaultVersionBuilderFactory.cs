using NoeticTools.Git2SemVer.Core.Logging;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Versioning.Builders;

internal sealed class DefaultVersionBuilderFactory : IDefaultVersionBuilderFactory
{
    private readonly ILogger _logger;

    public DefaultVersionBuilderFactory(ILogger logger)
    {
        _logger = logger;
    }

    public IVersionBuilder Create(SemVersion semanticVersion)
    {
        return new DefaultVersionBuilder(_logger);
    }
}