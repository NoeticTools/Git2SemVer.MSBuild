using Injectio.Attributes;
using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Tool.Commands.Add;
using NoeticTools.Git2SemVer.Tool.Commands.Remove;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.Commands;

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

    public IRemoveCommand CreateRemoveCommand()
    {
        return _servicesProvider.GetService<IRemoveCommand>()!;
    }
}