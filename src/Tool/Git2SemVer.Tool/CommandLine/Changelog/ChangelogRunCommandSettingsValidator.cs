using FluentValidation;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;

public sealed class ChangelogRunCommandSettingsValidator : AbstractValidator<ChangelogCommandSettings>
{
    public ChangelogRunCommandSettingsValidator()
    {
        RuleFor(settings => settings.Verbosity).Must(BeAValidVerbosity)
                                               .WithMessage("The verbosity must be 'Trace', 'Debug', 'Info', 'Warning', or 'Error'.");
    }

    private static bool BeAValidVerbosity(string verbosity)
    {
        return Enum.TryParse(verbosity, true, out LoggingLevel level);
    }
}