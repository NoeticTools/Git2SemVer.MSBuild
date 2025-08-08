using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

[RegisterSingleton]
internal class CommandFactory(IServiceProvider servicesProvider) : ICommandFactory
{
    public ISetupCommand CreateAddCommand()
    {
        return servicesProvider.GetService<ISetupCommand>()!;
    }

    public IChangelogCommand CreateChangelogCommand()
    {
        return servicesProvider.GetService<IChangelogCommand>()!;
    }

    public IRemoveCommand CreateRemoveCommand()
    {
        return servicesProvider.GetService<IRemoveCommand>()!;
    }

    public IRunCommand CreateRunCommand()
    {
        return servicesProvider.GetService<IRunCommand>()!;
    }
}