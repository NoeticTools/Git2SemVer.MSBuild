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
        return false;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        // Not supported
    }

    public void ReportBuildStatistic(string key, double value)
    {
        // Not supported
    }

    public void SetBuildLabel(string label)
    {
        // Not supported
    }
}