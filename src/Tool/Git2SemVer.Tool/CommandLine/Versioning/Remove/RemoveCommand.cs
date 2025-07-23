using System.Text;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Tools;
using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;

[RegisterSingleton]
internal sealed class RemoveCommand(
    ISolutionFinder solutionFinder,
    IDotNetTool dotNetCli,
    IConsoleIO console,
    IContentEditor contentEditor)
    : IRemoveCommand
{
    public bool HasError => console.HasError;

    public void Execute(string inputSolutionFile, bool unattended)
    {
        console.WriteMarkupInfoLine($"Removing Git2SemVer solution versioning{(unattended ? " (unattended)" : "")}.");
        console.WriteLine();

        var solution = solutionFinder.Find(inputSolutionFile);
        if (HasError)
        {
            return;
        }

        console.WriteMarkupLine($"""

                                 Ready to remove Git2SemVer versioning from [aqua]{solution!.Name}[/] solution. If the solution is currently open in Visual Studio, close it before proceeding.

                                 This is a best effort to remove all Git2SemVer files, projects, and settings from the solution. However customisation, renaming, or formatting may mean that some items may not be found. If so, this will be shown in the output as "No change." and some manual cleanup may be required.


                                 """);
        if (!unattended)
        {
            var proceed = console.PromptYesNo("Proceed?");
            console.WriteLine();
            if (!proceed)
            {
                console.WriteErrorLine("Aborted.");
            }
        }

        // todo - Get name of versioning project
        const string leaderProjectName = SolutionVersioningConstants.DefaultVersioningProjectName;
        var solutionDirectory = solution.Directory!;

        var changeMade = false;
        console.WriteMarkupInfoLine("Running:");
        console.WriteLine();
        changeMade |= RemoveDirectoryPropertiesInclude(solutionDirectory);
        changeMade |= DeleteDirectoryVersioningPropertiesFile(solutionDirectory);
        changeMade |= DeleteVersioningProjectFolder(solutionDirectory, leaderProjectName);
        changeMade |= RemoveVersioningProjectFromSolution(solution, leaderProjectName);

        if (HasError)
        {
            console.WriteLine();
            console.WriteErrorLine("Remove failed.");
            return;
        }

        console.WriteLine();
        console.WriteLine();
        if (changeMade)
        {
            console.WriteMarkupInfoLine("Done.");
        }
        else
        {
            console.WriteWarningLine("Nothing found to remove. Either manual removal is required or Git2SemVer was not added to this solution.");
        }
    }

    private bool DeleteDirectoryVersioningPropertiesFile(DirectoryInfo solutionDirectory)
    {
        var directoryVersioningPropsFile = solutionDirectory.WithFile(SolutionVersioningConstants.DirectoryVersionPropsFilename);
        if (directoryVersioningPropsFile.Exists)
        {
            directoryVersioningPropsFile.Delete();
            console.WriteMarkupInfoLine($"\t- Deleted properties file: '{directoryVersioningPropsFile.Name}.");
            return true;
        }

        console.WriteWarningLine($"\t- No change. Properties file '{directoryVersioningPropsFile.Name}' not found.");
        return false;
    }

    private bool DeleteVersioningProjectFolder(DirectoryInfo solutionDirectory, string leaderProjectName)
    {
        var versioningProjectDirectory = solutionDirectory.WithSubDirectory(leaderProjectName);
        if (versioningProjectDirectory.Exists)
        {
            versioningProjectDirectory.Delete(true);
            console.WriteMarkupInfoLine($"\t- Deleted project: '{leaderProjectName}.");
            return true;
        }

        console.WriteWarningLine($"\t- No change. Versioning project folder '{versioningProjectDirectory.Name}' not found.");
        return false;
    }

    private bool RemoveDirectoryPropertiesInclude(DirectoryInfo solutionDirectory)
    {
        var buildPropsFile = solutionDirectory.WithFile(SolutionVersioningConstants.DirectoryBuildPropsFilename);
        if (!buildPropsFile.Exists)
        {
            console.WriteWarningLine($"\t- No change. Properties file '{buildPropsFile.Name}' not found.");
            return false;
        }

        var existingContent = File.ReadAllText(buildPropsFile.FullName);
        const string
            includeLine =
                $"<Import Project=\"{SolutionVersioningConstants.DirectoryVersionPropsFilename}\"/>";

        if (existingContent.Contains(includeLine, StringComparison.CurrentCultureIgnoreCase))
        {
            var content = contentEditor.RemoveLinesWith(includeLine, existingContent);
            File.WriteAllText(buildPropsFile.FullName, content);
            //existingContent = existingContent.Replace(includeLine, "", StringComparison.CurrentCultureIgnoreCase);
            //File.WriteAllText(buildPropsFile.FullName, existingContent);
            console.WriteMarkupInfoLine($"\t- Updated '{buildPropsFile.Name}'.");
            return true;
        }

        console.WriteWarningLine($"\t- No change. '<Import Project=\"{SolutionVersioningConstants.DirectoryVersionPropsFilename}\"/>' not found in '{buildPropsFile.Name}'.");
        return false;
    }

    private bool RemoveVersioningProjectFromSolution(FileInfo solution, string leaderProjectName)
    {
        if (HasError)
        {
            return false;
        }

        var progResult = dotNetCli.Solution.GetProjects(solution.Name);
        if (!progResult.projects.Any(x => x.Equals($"{leaderProjectName}\\{leaderProjectName}.csproj", StringComparison.Ordinal)))
        {
            console.WriteWarningLine($"\t- No change. Project '{leaderProjectName}' not found in solution.");

            console.WriteLine();
            var projectsString = new StringBuilder("\t\t");
            projectsString.AppendJoin("\n\t\t", progResult.projects);
            console.WriteMarkupInfoLine(projectsString.ToString());
            return false;
        }

        var result = dotNetCli.Solution.RemoveProject(solution.Name, $"{leaderProjectName}/{leaderProjectName}.csproj");
        if (!HasError && result.returnCode == 0)
        {
            return true;
        }

        console.WriteErrorLine($"Unable to remove project '{leaderProjectName}' from solution '{solution.Name}'.");
        return false;
    }
}