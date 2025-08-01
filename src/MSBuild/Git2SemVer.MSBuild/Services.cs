using JetBrains.TeamCity.ServiceMessages.Write.Special;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.MSBuild.Tasks;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.MSBuild;

[RegisterSingleton]
internal sealed class Services
{
    public static IServiceProvider ConfigureServices(ILogger logger, TaskLoggingHelper taskLogging)
    {
        var services = new ServiceCollection();

        services.AddSingleton(logger);
        services.AddSingleton(new MSBuildTeamCityWriterFactory(taskLogging).Create());

        services.AddNoeticToolsGit2SemVerCore();
        services.AddNoeticToolsGit2SemVerFramework();

        return services.BuildServiceProvider();
    }
}