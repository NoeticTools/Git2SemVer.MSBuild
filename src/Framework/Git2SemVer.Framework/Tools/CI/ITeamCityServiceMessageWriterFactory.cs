using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

public interface ITeamCityServiceMessageWriterFactory
{
    ITeamCityWriter Create();
}