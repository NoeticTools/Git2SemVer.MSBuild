using System.Globalization;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Core.Tools.CI;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using ILogger = NoeticTools.Git2SemVer.Core.Logging.ILogger;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

[RegisterTransient]
internal sealed class TeamCityHost : BuildHostBase, IDetectableBuildHost
{
    private readonly ILogger _logger;
    private readonly ITeamCityWriter _serviceMessagesWriter;

    public TeamCityHost(ITeamCityWriter messageWriter, ILogger logger) : base(logger)
    {
        _logger = logger;
        Name = "TeamCity";
        var teamCityVersion = TeamCityHostSettings.Version;
        if (teamCityVersion.Length > 0)
        {
            BuildNumber = TeamCityHostSettings.BuildNumber;
        }

        BuildContext = "0";
        DefaultBuildNumberFunc = () => [BuildNumber];

        _serviceMessagesWriter = messageWriter;
    }

    public TeamCityHost(Action<string> buildOutput, ILogger logger)
        : this(new TeamCityServiceMessages().CreateWriter(buildOutput), logger)
    {
    }

    public HostTypeIds HostTypeId => HostTypeIds.TeamCity;

    public void Dispose()
    {
        _serviceMessagesWriter.Dispose();
    }

    public bool MatchesHostSignature()
    {
        return TeamCityHostSettings.IsHost();
    }

    public override void ReportBuildStatistic(string key, int value)
    {
        _logger.LogDebug($"Reporting build statistic {key} = {value}");
        using var writer = new TeamCityServiceMessages().CreateWriter(_logger.LogInfo);
        writer.WriteBuildStatistics(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public override void ReportBuildStatistic(string key, double value)
    {
        _logger.LogDebug($"Reporting build statistic {key} = {value:G13}");
        using var writer = new TeamCityServiceMessages().CreateWriter(_logger.LogInfo);
        writer.WriteBuildStatistics(key, $"{value:G13}");
    }

    public override void SetBuildLabel(string label)
    {
        _logger.LogDebug($"Setting TeamCity Build label to '{label}'.");
        using var writer = new TeamCityServiceMessages().CreateWriter(_logger.LogInfo);
        writer.WriteBuildNumber(label);
    }
}