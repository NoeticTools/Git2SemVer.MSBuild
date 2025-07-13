using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Tool.CommandLine;
using NoeticTools.Git2SemVer.Tool.Commands.Changelog;
using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Run;

internal sealed class RunCliCommand : CliCommandBase<RunCommandSettings>
{
    public override int Execute(CommandContext context, RunCommandSettings settings)
    {
        Validate(context);

        var commandFactory = GetCommandFactory(context, settings);

        var runner = commandFactory.CreateRunCommand();
        runner.Execute(settings);
        return (int)(runner.HasError ? ReturnCodes.CommandError : ReturnCodes.Succeeded);
    }
}