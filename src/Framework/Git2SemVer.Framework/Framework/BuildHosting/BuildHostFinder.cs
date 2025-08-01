using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.Config;
using NoeticTools.Git2SemVer.Framework.Tools.CI;


namespace NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;

internal class BuildHostFinder
{
    private readonly List<IBuildHost> _allBuildHosts;
    private readonly IReadOnlyList<IDetectableBuildHost> _detectableBuildHosts;
    private readonly ILogger _logger;

    public BuildHostFinder(IConfiguration config, 
                           ITeamCityWriter teamCityWriter, 
                           ILogger logger)
    {
        _logger = logger;
        _allBuildHosts =
        [
            // other supported hosts go here in order of detection precedence
            new TeamCityHost(teamCityWriter, logger),
            new GitHubHost(logger),
            new UncontrolledHost(config, logger),
            new CustomHost(logger)
        ];

        _detectableBuildHosts = _allBuildHosts.Where(x => x is IDetectableBuildHost).Cast<IDetectableBuildHost>().ToList();
    }

    public IBuildHost Find(string buildHostType)
    {
        if (!string.IsNullOrWhiteSpace(buildHostType))
        {
            if (!Enum.TryParse<HostTypeIds>(buildHostType, out var buildHostTypeId))
            {
                throw new
                    Git2SemVerConfigurationException($"Input Git2SemVer_HostType '{buildHostTypeId}' does not match a known host type.");
            }

            if (buildHostTypeId != HostTypeIds.Unknown)
            {
                var selectedHost = _allBuildHosts.Find(x => x.HostTypeId == buildHostTypeId)!;
                _logger.LogDebug($"Build host set by Git2SemVer_HostType is: '{selectedHost.Name}'");
                return selectedHost;
            }
        }

        var host = _detectableBuildHosts.First(x => x.MatchesHostSignature());
        _logger.LogDebug($"Building on a '{host.Name}' host.");
        return host;
    }
}