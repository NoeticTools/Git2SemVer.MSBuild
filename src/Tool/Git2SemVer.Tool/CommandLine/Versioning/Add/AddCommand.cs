using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Git2SemVer;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders;
using NoeticTools.Git2SemVer.Tool.MSBuild;
using NoeticTools.Git2SemVer.Tool.MSBuild.Projects;
using NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;

/// <summary>
///     Command to add solution versioning to a .net solution (sln).
/// </summary>
/// <param name="solutionFinder"></param>
/// <param name="userOptionsPrompt"></param>
/// <param name="dotNetCli"></param>
/// <param name="embeddedResources"></param>
/// <param name="projectDocumentReader"></param>
/// <param name="preconditionsValidator"></param>
/// <param name="console"></param>
/// <param name="logger"></param>
[RegisterSingleton]
internal sealed class AddCommand(
    ISolutionFinder solutionFinder,
    IUserOptionsPrompt userOptionsPrompt,
    IDotNetTool dotNetCli,
    IEmbeddedResources embeddedResources,
    IProjectDocumentReader projectDocumentReader,
    IAddPreconditionValidator preconditionsValidator,
    IConsoleIO console,
    ILogger logger)
    : ISetupCommand
{
    public bool HasError => console.HasError;

    public void Execute(string inputSolutionFile, bool unattended)
    {
        console.WriteMarkupInfoLine($"Adding Git2SemVer solution versioning{(unattended ? " (unattended)" : "")}.");
        console.WriteLine();

        var solution = solutionFinder.Find(inputSolutionFile);
        var solutionDirectory = solution!.Directory!;

        if (!preconditionsValidator.Validate(solutionDirectory, unattended))
        {
            return;
        }

        var userOptions = userOptionsPrompt.GetOptions(solution);
        if (console.HasError)
        {
            return;
        }

        console.WriteMarkupInfoLine("Running:");
        console.WriteLine();

        var propertiesDocument = AddVersioningPropsDocument(solutionDirectory);
        propertiesDocument.Properties["Git2SemVer_VersioningProjectName"].Value = userOptions.VersioningProjectName;
        propertiesDocument.Save();

        CreateVersioningProject(userOptions, solution);
        SetupGitIgnore(solutionDirectory);

        if (console.HasError)
        {
            console.WriteErrorLine("Add failed.");
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
            console.WriteMarkupDebugLine($"Overwriting file '{filename}'.");
        }

        embeddedResources.WriteResourceFile(filename, directory);
        console.WriteMarkupInfoLine($"\t- Added '{filename}' to solution directory.");
        return projectDocumentReader.Read(versioningPropsFile);
    }

    private void CreateSharedDirectory(DirectoryInfo parentDirectory)
    {
        var sharedDirectory = parentDirectory.WithSubDirectory(Git2SemVerConstants.DataFolderName);
        if (sharedDirectory.Exists)
        {
            logger.LogTrace("`{0}` already existed. Overwriting files in directory.", sharedDirectory.Name);
        }
        sharedDirectory.Create();

        var tempDataDirectory = parentDirectory.WithSubDirectory(Git2SemVerConstants.TemporaryDataFolderName);
        tempDataDirectory.Create();
        embeddedResources.WriteResourceFile(VersioningConstants.SharedVersionJsonPropertiesFilename, tempDataDirectory);

        console.WriteMarkupInfoLine($"\t- Added '{Git2SemVerConstants.DataFolderName}' shared directory to versioning project directory.");
    }

    private void CreateVersioningProject(UserOptions userOptions, FileInfo solution)
    {
        var projectName = userOptions.VersioningProjectName;
        dotNetCli.Projects.New("classlib", $"{projectName}");
        dotNetCli.Solution.AddProject(solution.Name, $"{projectName}/{projectName}.csproj");
        var csxFileDestination = solution.Directory!.WithSubDirectory(projectName).WithFile(VersioningConstants.DefaultScriptFilename);
        embeddedResources.WriteResourceFile(VersioningConstants.DefaultScriptFilename, csxFileDestination.FullName);
        console.WriteMarkupInfoLine($"\t- Added '{projectName}' project to solution.");

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
                console.WriteWarningLine($"Existing '{buildPropsFile.FullName}' already has {SolutionVersioningConstants.DirectoryVersionPropsFilename} import.");
            }
            else
            {
                File.WriteAllText(buildPropsFile.FullName, existingContent.Replace("</Project>",
                                                                                   $"""
                                                                                        <Import Project="{SolutionVersioningConstants.DirectoryVersionPropsFilename}"/>
                                                                                    </Project>
                                                                                    """,
                                                                                   StringComparison.Ordinal));
                console.WriteMarkupInfoLine($"\t- Updated '{buildPropsFile.Name}'.");
            }
        }
        else
        {
            File.WriteAllText(buildPropsFile.FullName, $"""
                                                        <Project>
                                                            <Import Project="{SolutionVersioningConstants.DirectoryVersionPropsFilename}"/>
                                                        </Project>
                                                        """);
            console.WriteMarkupInfoLine($"\t- Added '{buildPropsFile.Name}' file to solution directory.");
        }
    }

    private void SetupGitIgnore(DirectoryInfo directory)
    {
        var gitIgnoreFile = directory.WithFile(".gitignore");
        if (gitIgnoreFile.Exists)
        {
            var fullName = gitIgnoreFile.FullName;
            var content = File.ReadAllText(fullName);
            if (content.Contains(Git2SemVerConstants.TemporaryDataFolderName, StringComparison.Ordinal))
            {
                console.WriteWarningLine($"The .gitignore file already had an entry for {Git2SemVerConstants.TemporaryDataFolderName}.");
                return;
            }

            content += $"""

                        # Generated version properties file
                        {Git2SemVerConstants.TemporaryDataFolderName}/{VersioningConstants.SharedVersionJsonPropertiesFilename}

                        """;
            File.WriteAllText(fullName, content);
            console.WriteMarkupInfoLine($"\t- Added generated version properties file '{VersioningConstants.SharedVersionJsonPropertiesFilename}' to .gitignore file.");
        }
        else
        {
            logger.LogDebug(".gitignore file not found.");
        }
    }
}