using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal class TeamCityHost : BuildHostBase, IDetectableBuildHost
{
    private readonly ILogger _logger;
    private readonly string _teamCityVersion;
    private const string TeamCityVersionEnvVarName = "TEAMCITY_VERSION";
    private const string BuildNumberEnvVarName = "BUILD_NUMBER";
    private const string BuildNumberCacheEnvVarName = "BUILD_NUMBER_CACHE";

    public TeamCityHost(ILogger logger) : base(logger)
    {
        _logger = logger;
        _teamCityVersion = Environment.GetEnvironmentVariable(TeamCityVersionEnvVarName) ?? "";
        BuildNumber = (_teamCityVersion.Length > 0) ? GetBuildNumber(logger) : "";
        BuildContext = "0";
        DefaultBuildNumberFunc = () => [BuildNumber];
    }

    private static string GetBuildNumber(ILogger logger)
    {
        var buildNumberVariable = Environment.GetEnvironmentVariable(BuildNumberEnvVarName);
        if (!int.TryParse(buildNumberVariable!, out var buildNumber))
        {
            if (int.TryParse(Environment.GetEnvironmentVariable(BuildNumberCacheEnvVarName)!, out buildNumber))
            {
                return buildNumber.ToString();
            }

            logger.LogError($"Unable to read build number. {BuildNumberEnvVarName} is '{buildNumberVariable}'");
            return "";
        }

        logger.LogInfo("================================================================================");//>>>
        Environment.SetEnvironmentVariable(BuildNumberCacheEnvVarName, buildNumberVariable);
        return "";
    }

    public HostTypeIds HostTypeId => HostTypeIds.TeamCity;

    public string Name => "TeamCity";

    public string BumpBuildNumber()
    {
        // Not supported - do nothing
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