using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Framework.Config;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

[RegisterTransient]
public sealed class BuildHostFactory(
    ILocalSettings config,
    ITeamCityWriter teamCityWriter,
    ILogger logger) : IBuildHostFactory
{
    public IBuildHost Create(string hostType, string buildNumber, string buildContext, string inputsBuildIdFormat)
    {
        var host = new BuildHost(new BuildHostFinder(config, teamCityWriter, logger).Find(hostType), logger);

        if (!string.IsNullOrWhiteSpace(buildNumber))
        {
            host.BuildNumber = buildNumber;
        }

        if (!string.IsNullOrWhiteSpace(buildContext))
        {
            host.BuildContext = buildContext;
        }

        if (!string.IsNullOrWhiteSpace(inputsBuildIdFormat))
        {
            host.BuildIdFormat = inputsBuildIdFormat;
        }

        return host;
    }
}