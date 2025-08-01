using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Framework.Tools.CI;


namespace NoeticTools.Git2SemVer.IntegrationTests.Framework;

public sealed class TeamCityWriterFactoryStub : ITeamCityServiceMessageWriterFactory
{
    public ITeamCityWriter Create()
    {
        return new TeamCityServiceMessages().CreateWriter(Console.Out.WriteLine);
    }
}