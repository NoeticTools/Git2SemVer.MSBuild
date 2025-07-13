using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Tool.CommandLine;
using NoeticTools.Git2SemVer.Tool.Commands.Changelog;
using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;

internal class AddCliCommand : CliCommandBase<SolutionCommandSettings>
{
    public override int Execute(CommandContext context, SolutionCommandSettings settings)
    {
        Validate(context);

        var commandFactory = GetCommandFactory(context, settings);

        var runner = commandFactory.CreateAddCommand();
        runner.Execute(settings.SolutionName, settings.Unattended);
        return (int)(runner.HasError ? ReturnCodes.CommandError : ReturnCodes.Succeeded);
    }
}