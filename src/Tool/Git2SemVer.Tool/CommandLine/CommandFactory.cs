using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

[RegisterSingleton]
internal class CommandFactory : ICommandFactory
{
    private readonly IServiceProvider _servicesProvider;

    public CommandFactory(IServiceProvider servicesProvider)
    {
        _servicesProvider = servicesProvider;
    }

    public ISetupCommand CreateAddCommand()
    {
        return _servicesProvider.GetService<ISetupCommand>()!;
    }

    public IChangelogCommand CreateChangelogCommand()
    {
        return _servicesProvider.GetService<IChangelogCommand>()!;
    }

    public IRemoveCommand CreateRemoveCommand()
    {
        return _servicesProvider.GetService<IRemoveCommand>()!;
    }

    public IRunCommand CreateRunCommand()
    {
        return _servicesProvider.GetService<IRunCommand>()!;
    }
}