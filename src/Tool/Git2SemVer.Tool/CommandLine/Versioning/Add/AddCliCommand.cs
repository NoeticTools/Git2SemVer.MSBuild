using NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;
using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

internal class AddCliCommand : CliCommandBase<SolutionCommandSettings>
{
    public override int Execute(CommandContext context, SolutionCommandSettings settings)
    {
        Validate(context);

        var commandFactory = GetCommandFactory(context, settings);

        var runner = commandFactory.CreateAddCommand();
        runner.Execute(settings.SolutionName, !settings.Confirm);
        return (int)(runner.HasError ? ReturnCodes.CommandError : ReturnCodes.Succeeded);
    }
}