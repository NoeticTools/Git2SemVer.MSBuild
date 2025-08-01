using JetBrains.TeamCity.ServiceMessages.Write.Special;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Framework.Tools.CI;


namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

public sealed class MSBuildTeamCityWriterFactory(TaskLoggingHelper taskLogging) : ITeamCityServiceMessageWriterFactory
{
    public ITeamCityWriter Create()
    {
        return new TeamCityServiceMessages().CreateWriter(msg => taskLogging.LogMessage(MessageImportance.High, msg));
    }
}