using JetBrains.TeamCity.ServiceMessages.Write.Special;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

public interface ITeamCityServiceMessageWriterFactory
{
    ITeamCityWriter Create();
}