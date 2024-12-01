﻿using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Versioning.Generation.GitHistoryWalking;


namespace NoeticTools.Git2SemVer.Versioning.Generation.Builders;

internal sealed class DefaultVersionBuilderFactory : IDefaultVersionBuilderFactory
{
    private readonly ILogger _logger;

    public DefaultVersionBuilderFactory(ILogger logger)
    {
        _logger = logger;
    }

    public IVersionBuilder Create(HistoryPaths historyPaths)
    {
        return new DefaultVersionBuilder(historyPaths, _logger);
    }
}