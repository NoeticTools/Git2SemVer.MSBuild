using System.Globalization;
using System.Text;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Git2SemVer;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using NoeticTools.Git2SemVer.Tool.MSBuild.Projects;
using Spectre.Console;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

[RegisterSingleton]
internal sealed class AddPreconditionValidator(
    IProjectDocumentReader projectDocumentReader,
    IConsoleIO console,
    ILogger logger)
    : IAddPreconditionValidator
{
    public bool Validate(DirectoryInfo directory, bool unattended)
    {
        if (!ValidateCritical(directory))
        {
            return false;
        }

        return ValidateNonCritical(directory, unattended);
    }

    private bool ValidateCritical(DirectoryInfo directory)
    {
        var buildPropsFile = directory.WithFile(SolutionVersioningConstants.DirectoryBuildPropsFilename);
        if (!buildPropsFile.Exists)
        {
            return true;
        }

        var existingContent = File.ReadAllText(buildPropsFile.FullName);
        if (!existingContent.Contains("\"Directory.Versioning.Build.Props\"", StringComparison.Ordinal) ||
            existingContent.Contains("<Import Project=\"Directory.Versioning.Build.Props\"/>", StringComparison.Ordinal))
        {
            return true;
        }

        console.WriteMarkupErrorLine($"[error]The properties file '[em]{buildPropsFile.Name}[/]' contains a \"Directory.Versioning.Build.Props\" entry. Remove manually.[/]");
        return false;
    }

    private bool ValidateNonCritical(DirectoryInfo directory, bool unattended)
    {
        var priorSetupEvidence = new List<string>();

        var shareDirectory = directory.WithSubDirectory(Git2SemVerConstants.DataFolderName);
        if (shareDirectory.Exists)
        {
            priorSetupEvidence.Add($"The Git2SemVer share directory `[em]{Git2SemVerConstants.DataFolderName}[/]` already exists.");
        }

        var versioningProjectDirectory = directory.WithSubDirectory(SolutionVersioningConstants.DefaultVersioningProjectName);
        if (versioningProjectDirectory.Exists)
        {
            priorSetupEvidence
                .Add($"The default versioning project directory '[em]{versioningProjectDirectory.Name}[/]' exists.");
        }

        var versioningPropsFile = directory.WithFile(SolutionVersioningConstants.DirectoryVersionPropsFilename);
        if (versioningPropsFile.Exists)
        {
            priorSetupEvidence.Add($"The properties file '[em]{versioningPropsFile.Name}[/]' already exists.");

            var propertiesDocument = projectDocumentReader.Read(versioningPropsFile);
            if (propertiesDocument.Properties["Git2SemVer_Installed"].BoolValue)
            {
                priorSetupEvidence.Add("The properties file's installed flag (`Git2SemVer_Installed`) is true.");
            }
        }

        if (priorSetupEvidence.Count == 0)
        {
            logger.LogTrace("No prior setup evidence found.");
            return true;
        }

        var message = new StringBuilder();
        message.AppendLine("It looks like the solution is, or was, setup for Git2SemVer solution versioning as:\n");
        foreach (var evidence in priorSetupEvidence)
        {
            message.AppendLine(CultureInfo.InvariantCulture, $"  - {evidence}");
        }

        message.AppendLine();
        if (unattended)
        {
            message.AppendLine("[error]Aborting as in unattended mode.[/]");
            console.WriteMarkupErrorLine(message.ToString());
            return false;
        }

        message.AppendLine("This may not be a problem but this setup may fail if the prior setup has not been fully removed.");
        console.WriteMarkupWarningLine("[warn]" + message + "[/]");

        Console.WriteLine();
        var proceed = AnsiConsole.Prompt(new TextPrompt<bool>("Proceed?")
                                         .AddChoices([true, false])
                                         .DefaultValue(true)
                                         .WithConverter(choice => choice ? "y" : "n"));
        Console.WriteLine();
        return proceed;
    }
}