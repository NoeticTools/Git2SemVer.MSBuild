using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Git2SemVer;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using NoeticTools.Git2SemVer.Tool.MSBuild.Projects;
using NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;


namespace NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;

[RegisterSingleton]
internal sealed class AddCommand : ISetupCommand
{
    private readonly IConsoleIO _console;
    private readonly IDotNetTool _dotNetCli;
    private readonly IEmbeddedResources _embeddedResources;
    private readonly ILogger _logger;
    private readonly IAddPreconditionValidator _preconditionsValidator;
    private readonly IProjectDocumentReader _projectDocumentReader;
    private readonly ISolutionFinder _solutionFinder;
    private readonly IUserOptionsPrompt _userOptionsPrompt;

    public AddCommand(ISolutionFinder solutionFinder,
                      IUserOptionsPrompt userOptionsPrompt,
                      IDotNetTool dotNetCli,
                      IEmbeddedResources embeddedResources,
                      IProjectDocumentReader projectDocumentReader,
                      IAddPreconditionValidator preconditionsValidator,
                      IConsoleIO console,
                      ILogger logger)
    {
        _solutionFinder = solutionFinder;
        _userOptionsPrompt = userOptionsPrompt;
        _dotNetCli = dotNetCli;
        _embeddedResources = embeddedResources;
        _projectDocumentReader = projectDocumentReader;
        _preconditionsValidator = preconditionsValidator;
        _console = console;
        _logger = logger;
    }

    public bool HasError => _console.HasError;

    public void Execute(string inputSolutionFile, bool unattended)
    {
        _console.WriteMarkupInfoLine($"Adding Git2SemVer solution versioning{(unattended ? " (unattended)" : "")}.");
        _console.WriteLine();

        var solution = _solutionFinder.Find(inputSolutionFile);
        if (_console.HasError)
        {
            _console.WriteErrorLine("Error finding solution file.");
            return;
        }

        var solutionDirectory = solution!.Directory!;

        if (!_preconditionsValidator.Validate(solutionDirectory, unattended))
        {
            return;
        }

        var userOptions = _userOptionsPrompt.GetOptions(solution);
        if (_console.HasError)
        {
            return;
        }

        _console.WriteMarkupInfoLine("Running:");
        _console.WriteLine();

        var propertiesDocument = AddVersioningPropsDocument(solutionDirectory);
        propertiesDocument.Properties["Git2SemVer_VersioningProjectName"].Value = userOptions.VersioningProjectName;
        propertiesDocument.Save();

        CreateVersioningProject(userOptions, solution);
        SetupGitIgnore(solutionDirectory);

        if (_console.HasError)
        {
            _console.WriteErrorLine("Add failed.");
            return;
        }

        propertiesDocument.Properties["Git2SemVer_Enabled"].BoolValue = true;
        propertiesDocument.Properties["Git2SemVer_Installed"].BoolValue = true;
        propertiesDocument.Save();

        Console.WriteLine("\nDone, enjoy");
    }

    private ProjectDocument AddVersioningPropsDocument(DirectoryInfo directory)
    {
        PrepareDirectoryBuildPropsFile(directory);

        const string filename = SolutionVersioningConstants.DirectoryVersionPropsFilename;
        var versioningPropsFile = directory.WithFile(filename);
        if (versioningPropsFile.Exists)
        {
            _console.WriteMarkupDebugLine($"Overwriting file '{filename}'.");
        }

        _embeddedResources.WriteResourceFile(filename, directory);
        _console.WriteMarkupInfoLine($"\t- Added '{filename}' to solution directory.");
        return _projectDocumentReader.Read(versioningPropsFile);
    }

    private void CreateSharedDirectory(DirectoryInfo parentDirectory)
    {
        var sharedDirectory = parentDirectory.WithSubDirectory(Git2SemVerConstants.ShareFolderName);
        if (sharedDirectory.Exists)
        {
            _logger.LogTrace("`{0}` already existed. Overwriting files in directory.", sharedDirectory.Name);
        }

        sharedDirectory.Create();

        _embeddedResources.WriteResourceFile(Git2SemVerConstants.SharedVersionJsonPropertiesFilename, sharedDirectory);

        _console.WriteMarkupInfoLine($"\t- Added '{Git2SemVerConstants.ShareFolderName}' shared directory to versioning project directory.");
    }

    private void CreateVersioningProject(UserOptions userOptions, FileInfo solution)
    {
        var projectName = userOptions.VersioningProjectName;
        _dotNetCli.Projects.New("classlib", $"{projectName}");
        _dotNetCli.Solution.AddProject(solution.Name, $"{projectName}/{projectName}.csproj");
        var csxFileDestination = solution.Directory!.WithSubDirectory(projectName).WithFile(Git2SemVerConstants.DefaultScriptFilename);
        _embeddedResources.WriteResourceFile(Git2SemVerConstants.DefaultScriptFilename, csxFileDestination.FullName);
        _console.WriteMarkupInfoLine($"\t- Added '{projectName}' project to solution.");

        var versioningProjectDirectory = solution.Directory!.WithSubDirectory(userOptions.VersioningProjectName);
        CreateSharedDirectory(versioningProjectDirectory);
    }

    private void PrepareDirectoryBuildPropsFile(DirectoryInfo directory)
    {
        var buildPropsFile = directory.WithFile(SolutionVersioningConstants.DirectoryBuildPropsFilename);
        if (buildPropsFile.Exists)
        {
            var existingContent = File.ReadAllText(buildPropsFile.FullName);
            if (existingContent.Contains($"<Import Project=\"{SolutionVersioningConstants.DirectoryVersionPropsFilename}\"/>",
                                         StringComparison.Ordinal))
            {
                _console.WriteWarningLine($"Existing '{buildPropsFile.FullName}' already has {SolutionVersioningConstants.DirectoryVersionPropsFilename} import.");
            }
            else
            {
                File.WriteAllText(buildPropsFile.FullName, existingContent.Replace("</Project>",
                                                                                   $"""
                                                                                        <Import Project="{SolutionVersioningConstants.DirectoryVersionPropsFilename}"/>
                                                                                    </Project>
                                                                                    """,
                                                                                   StringComparison.Ordinal));
                _console.WriteMarkupInfoLine($"\t- Updated '{buildPropsFile.Name}'.");
            }
        }
        else
        {
            File.WriteAllText(buildPropsFile.FullName, $"""
                                                        <Project>
                                                            <Import Project="{SolutionVersioningConstants.DirectoryVersionPropsFilename}"/>
                                                        </Project>
                                                        """);
            _console.WriteMarkupInfoLine($"\t- Added '{buildPropsFile.Name}' file to solution directory.");
        }
    }

    private void SetupGitIgnore(DirectoryInfo directory)
    {
        var gitIgnoreFile = directory.WithFile(".gitignore");
        if (gitIgnoreFile.Exists)
        {
            var fullName = gitIgnoreFile.FullName;
            var content = File.ReadAllText(fullName);
            if (content.Contains(Git2SemVerConstants.ShareFolderName, StringComparison.Ordinal))
            {
                _console.WriteWarningLine($"The .gitignore file already had an entry for {Git2SemVerConstants.ShareFolderName}.");
                return;
            }

            content += $"""

                        # Generated version properties file
                        {Git2SemVerConstants.ShareFolderName}/{Git2SemVerConstants.SharedVersionJsonPropertiesFilename}

                        """;
            File.WriteAllText(fullName, content);
            _console.WriteMarkupInfoLine($"\t- Added generated version properties file '{Git2SemVerConstants.SharedVersionJsonPropertiesFilename}' to .gitignore file.");
        }
        else
        {
            _logger.LogDebug(".gitignore file not found.");
        }
    }
}