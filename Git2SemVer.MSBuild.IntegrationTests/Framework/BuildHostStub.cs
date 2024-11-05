using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;

internal class BuildHostStub : IBuildHost
{
    private readonly ILogger _logger;

    public BuildHostStub(ILogger logger)
    {
        _logger = logger;
    }

    public string BuildContext { get; set; } = "";

    public IReadOnlyList<string> BuildId { get; } = [];

    public string BuildIdFormat { get; set; } = "";

    public string BuildNumber { get; set; } = "42";

    public HostTypeIds HostTypeId { get; set; } = HostTypeIds.Unknown;

    public string Name { get; set; } = "Test Host";

    public string BumpBuildNumber()
    {
        return BuildNumber;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        _logger.LogTrace($"Build statistic {key} = {value}");
    }

    public void ReportBuildStatistic(string key, double value)
    {
        _logger.LogTrace($"Build statistic {key} = {value:G13}");
    }

    public void SetBuildLabel(string label)
    {
        _logger.LogInfo($"SetBuildLabel: {label}");
    }
}