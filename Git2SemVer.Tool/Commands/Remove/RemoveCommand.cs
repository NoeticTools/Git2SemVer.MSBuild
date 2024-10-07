using System.Text;
using Injectio.Attributes;
using NoeticTools.Common;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools;
using NoeticTools.Common.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Tool.Commands.Add;
using NoeticTools.Git2SemVer.Tool.Framework;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;


namespace NoeticTools.Git2SemVer.Tool.Commands.Remove;

[RegisterSingleton]
internal sealed class RemoveCommand : IRemoveCommand
{
    private readonly IConsoleIO _console;
    private readonly IContentEditor _contentEditor;
    private readonly IDotNetTool _dotNetCli;
    private readonly ISolutionFinder _solutionFinder;

    public RemoveCommand(ISolutionFinder solutionFinder,
                         IUserOptionsPrompt userOptionsPrompt,
                         IDotNetTool dotNetCli,
                         IConsoleIO console,
                         IContentEditor contentEditor,
                         ILogger logger)
    {
        UserOptionsPrompt = userOptionsPrompt;
        _solutionFinder = solutionFinder;
        _dotNetCli = dotNetCli;
        _console = console;
        _contentEditor = contentEditor;
    }

    public bool HasError => _console.HasError;

    public IUserOptionsPrompt UserOptionsPrompt { get; }

    public void Execute(string inputSolutionFile, bool unattended)
    {
        _console.WriteInfoLine($"Removing Git2SemVer solution versioning{(unattended ? " (unattended)" : "")}.");
        _console.WriteLine();

        var solution = _solutionFinder.Find(inputSolutionFile);
        if (HasError)
        {
            return;
        }

        _console.MarkupLine($"""

                             Ready to remove Git2SemVer versioning from [aqua]{solution!.Name}[/] solution. If the solution is currently open in Visual Studio, close it before proceeding.

                             This is a best effort to remove all Git2SemVer files, projects, and settings from the solution. However customisation, renaming, or formatting may mean that some items may not be found. If so, this will be shown in the output as "No change." and some manual cleanup may be required.


                             """);
        if (!unattended)
        {
            var proceed = _console.PromptYesNo("Proceed?");
            _console.WriteLine();
            if (!proceed)
            {
                _console.WriteErrorLine("Aborted.");
            }
        }

        // todo - Get name of versioning project
        const string leaderProjectName = SolutionVersioningConstants.DefaultVersioningProjectName;
        var solutionDirectory = solution!.Directory!;

        var changeMade = false;
        _console.WriteInfoLine("Running:");
        _console.WriteLine();
        changeMade |= RemoveDirectoryPropertiesInclude(solutionDirectory);
        changeMade |= DeleteDirectoryVersioningPropertiesFile(solutionDirectory);
        changeMade |= DeleteVersioningProjectFolder(solutionDirectory, leaderProjectName);
        changeMade |= RemoveVersioningProjectFromSolution(solution, leaderProjectName);

        if (HasError)
        {
            _console.WriteLine();
            _console.WriteErrorLine("Remove failed.");
            return;
        }

        _console.WriteLine();
        _console.WriteLine();
        if (changeMade)
        {
            _console.WriteInfoLine("Done.");
        }
        else
        {
            _console.WriteWarningLine("Nothing found to remove. Either manual removal is required or Git2SemVer was not added to this solution.");
        }
    }

    private bool DeleteDirectoryVersioningPropertiesFile(DirectoryInfo solutionDirectory)
    {
        var directoryVersioningPropsFile = solutionDirectory.WithFile(SolutionVersioningConstants.DirectoryVersionPropsFilename);
        if (directoryVersioningPropsFile.Exists)
        {
            directoryVersioningPropsFile.Delete();
            _console.WriteInfoLine($"\t- Deleted properties file: '{directoryVersioningPropsFile.Name}.");
            return true;
        }

        _console.WriteWarningLine($"\t- No change. Properties file '{directoryVersioningPropsFile.Name}' not found.");
        return false;
    }

    private bool DeleteVersioningProjectFolder(DirectoryInfo solutionDirectory, string leaderProjectName)
    {
        var versioningProjectDirectory = solutionDirectory.WithSubDirectory(leaderProjectName);
        if (versioningProjectDirectory.Exists)
        {
            versioningProjectDirectory.Delete(true);
            _console.WriteInfoLine($"\t- Deleted project: '{leaderProjectName}.");
            return true;
        }

        _console.WriteWarningLine($"\t- No change. Versioning project folder '{versioningProjectDirectory.Name}' not found.");
        return false;
    }

    private bool RemoveDirectoryPropertiesInclude(DirectoryInfo solutionDirectory)
    {
        var buildPropsFile = solutionDirectory.WithFile(SolutionVersioningConstants.DirectoryBuildPropsFilename);
        if (!buildPropsFile.Exists)
        {
            _console.WriteWarningLine($"\t- No change. Properties file '{buildPropsFile.Name}' not found.");
            return false;
        }

        var existingContent = File.ReadAllText(buildPropsFile.FullName);
        const string
            includeLine =
                $"<Import Project=\"{SolutionVersioningConstants.DirectoryVersionPropsFilename}\"/>";

        if (existingContent.Contains(includeLine, StringComparison.CurrentCultureIgnoreCase))
        {
            var content = _contentEditor.RemoveLinesWith(includeLine, existingContent);
            File.WriteAllText(buildPropsFile.FullName, content);
            //existingContent = existingContent.Replace(includeLine, "", StringComparison.CurrentCultureIgnoreCase);
            //File.WriteAllText(buildPropsFile.FullName, existingContent);
            _console.WriteInfoLine($"\t- Updated '{buildPropsFile.Name}'.");
            return true;
        }

        _console.WriteWarningLine($"\t- No change. '<Import Project=\"{SolutionVersioningConstants.DirectoryVersionPropsFilename}\"/>' not found in '{buildPropsFile.Name}'.");
        return false;
    }

    private bool RemoveVersioningProjectFromSolution(FileInfo solution, string leaderProjectName)
    {
        if (HasError)
        {
            return false;
        }

        var progResult = _dotNetCli.Solution.GetProjects(solution.Name);
        if (!progResult.projects.Any(x => x.Equals($"{leaderProjectName}\\{leaderProjectName}.csproj")))
        {
            _console.WriteWarningLine($"\t- No change. Project '{leaderProjectName}' not found in solution.");

            _console.WriteLine();
            var projectsString = new StringBuilder("\t\t");
            projectsString.AppendJoin("\n\t\t", progResult.projects);
            _console.WriteInfoLine(projectsString.ToString());
            return false;
        }

        var result = _dotNetCli.Solution.RemoveProject(solution.Name, $"{leaderProjectName}/{leaderProjectName}.csproj");
        if (!HasError && result.returnCode == 0)
        {
            return true;
        }

        _console.WriteErrorLine($"Unable to remove project '{leaderProjectName}' from solution '{solution.Name}'.");
        return false;
    }
}