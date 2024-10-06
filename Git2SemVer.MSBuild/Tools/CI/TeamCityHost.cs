using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal class TeamCityHost : BuildHostBase, IDetectableBuildHost
{
    private readonly ILogger _logger;
    private readonly string _teamCityVersion;

    public TeamCityHost(ILogger logger)
    {
        _logger = logger;
        _teamCityVersion = Environment.GetEnvironmentVariable("TEAMCITY_VERSION") ?? "";
        if (!int.TryParse(Environment.GetEnvironmentVariable("BUILD_NUMBER")!, out var buildNumber))
        {
            BuildNumber = "";
            return;
        }

        BuildNumber = buildNumber.ToString();
        BuildContext = "0";
        DefaultBuildNumberFunc = () => [BuildNumber];
    }

    public HostTypeIds HostTypeId => HostTypeIds.TeamCity;

    public string Name => "TeamCity";

    public string BumpBuildNumber()
    {
        // Not supported
        return BuildNumber;
    }

    public bool MatchesHostSignature()
    {
        var result = !string.IsNullOrWhiteSpace(_teamCityVersion) &&
                     !string.IsNullOrWhiteSpace(BuildNumber);
        if (result)
        {
            _logger.LogInfo("Detected build running on TeamCity.");
        }

        return result;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        _logger.LogInfo($"Build statistic {key} = {value}");
        _logger.LogInfo($"##teamcity[buildStatisticValue key='{key}' value='{value}']");
    }

    public void ReportBuildStatistic(string key, double value)
    {
        _logger.LogInfo($"Build statistic {key} = {value:G13}");
        _logger.LogInfo($"##teamcity[buildStatisticValue key='{key}' value='{value:G13}']");
    }

    public void SetBuildLabel(string label)
    {
        _logger.LogInfo($"Setting TeamCity Build label to '{label}'.");
        _logger.LogInfo($"##teamcity[buildNumber '{label}']");
    }
}