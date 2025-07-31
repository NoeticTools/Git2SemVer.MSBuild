using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;

internal sealed class BuildHost : IBuildHost
{
    private readonly IBuildHost _inner;
    private readonly ILogger _logger;

    internal BuildHost(IBuildHost inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public string BuildContext
    {
        get => _inner.BuildContext;
        set
        {
            _inner.BuildContext = value;
            _logger.LogDebug("Build context is {0}.", value);
        }
    }

    public IReadOnlyList<string> BuildId => _inner.BuildId;

    public string BuildIdFormat
    {
        get => _inner.BuildIdFormat;
        set
        {
            _inner.BuildIdFormat = value;
            _logger.LogDebug("Build ID format is {0}.", value);
        }
    }

    public string BuildNumber
    {
        get => _inner.BuildNumber;
        set
        {
            _inner.BuildNumber = value;
            _logger.LogDebug("Build number is {0}.", value);
        }
    }

    public HostTypeIds HostTypeId => _inner.HostTypeId;

    public string Name => _inner.Name;

    public string BumpBuildNumber()
    {
        return _inner.BumpBuildNumber();
    }

    public void Dispose()
    {
        _inner.Dispose();
    }

    public void ReportBuildStatistic(string key, int value)
    {
        _inner.ReportBuildStatistic(key, value);
    }

    public void ReportBuildStatistic(string key, double value)
    {
        _inner.ReportBuildStatistic(key, value);
    }

    public void SetBuildLabel(string label)
    {
        _inner.SetBuildLabel(label);
    }
}