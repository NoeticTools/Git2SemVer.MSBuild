using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;

internal class ChangelogCliCommand : CliCommandBase<ChangelogCommandSettings>
{
    public override int Execute(CommandContext context, ChangelogCommandSettings settings)
    {
        Validate(context);
        if (!settings.Validate().Successful)
        {
            return (int)ExitCodes.InvalidCommandSettingsError;
        }

        var commandFactory = GetCommandFactory(context, settings);

        var runner = commandFactory.CreateChangelogCommand();
        runner.Execute(settings);
        return (int)(runner.HasError ? ReturnCodes.CommandError : ReturnCodes.Succeeded);
    }
}