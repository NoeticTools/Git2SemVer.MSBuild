using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

public sealed class TeamCityLoggerWriterFactory(ILogger logger) : ITeamCityServiceMessageWriterFactory
{
    public ITeamCityWriter Create()
    {
        return new TeamCityServiceMessages().CreateWriter(logger.LogInfo);
    }
}