using Injectio.Attributes;
using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.Tool.Framework;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using Spectre.Console;


namespace NoeticTools.Git2SemVer.Tool.Commands.Add;

[RegisterSingleton]
internal sealed class UserAddSolutionVersioningOptionsPrompt : IUserOptionsPrompt
{
    private readonly IConsoleIO _console;
    private readonly ILogger _logger;

    public UserAddSolutionVersioningOptionsPrompt(IConsoleIO console, ILogger logger)
    {
        _console = console;
        _logger = logger;
    }

    public UserOptions GetOptions(FileInfo solution)
    {
        _console.MarkupLine($"Projects in the current directory and sub directories will be configured to use Git2SemVer solution versioning. A versioning project will be added to the solution '[em]{solution.Name}[/]' and solution versioning directory properties files will be added to the current directory.");

        var leadingProjectName = _console.Prompt(new TextPrompt<string>("Versioning project name?")
                                                     .Validate(folderName => ValidateFolderDoesNotExist(folderName, solution.Directory!)),
                                                 SolutionVersioningConstants.DefaultVersioningProjectName);

        var versionTagPrefix = _console.Ask<string>("Release git tag prefix?", "v");

        _console.WriteLine();
        var options = new UserOptions(leadingProjectName, versionTagPrefix);
        _console.MarkupLine($"Ready to add Git2SemVer versioning to '[em]{solution.Name}[/]' solution. If the solution is currently open in Visual Studio, close it before proceeding.");
        var proceed = _console.Prompt(new TextPrompt<bool>("Proceed?")
                                      .AddChoices([true, false])
                                      .WithConverter(choice => choice ? "y" : "n"),
                                      true);
        Console.WriteLine();
        if (!proceed)
        {
            _logger.LogError("Aborted.");
        }

        return options;
    }

    private ValidationResult ValidateFolderDoesNotExist(string folder, DirectoryInfo baseDirectory)
    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            return ValidationResult.Error("A value is required.");
        }

        var path = Path.Combine(baseDirectory.FullName, folder);
        if (!Directory.Exists(path))
        {
            return ValidationResult.Success();
        }

        return ValidationResult.Error("That folder exists.");
    }
}