using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal class CustomHost : BuildHostBase, IDetectableBuildHost
{
    private readonly ILogger _logger;

    public CustomHost(ILogger logger) : base(logger)
    {
        _logger = logger;
        BuildNumber = "UNKNOWN";
        BuildContext = "UNKNOWN";
        DefaultBuildNumberFunc = () => [BuildContext, BuildNumber];
    }

    public HostTypeIds HostTypeId => HostTypeIds.Custom;

    public string Name => "Custom";

    public string BumpBuildNumber()
    {
        // Not supported
        return BuildNumber;
    }

    public bool MatchesHostSignature()
    {
        // Not detectable
        return false;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        _logger.LogTrace("Custom host does not support build statistics.");
    }

    public void ReportBuildStatistic(string key, double value)
    {
        _logger.LogTrace("Custom host does not support build statistics.");
    }

    public void SetBuildLabel(string label)
    {
        _logger.LogTrace("Custom host does not setting a build label.");
    }
}