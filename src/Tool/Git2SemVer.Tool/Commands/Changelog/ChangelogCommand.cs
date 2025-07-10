using System.Diagnostics;
using System.Xml.Linq;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Run;


namespace NoeticTools.Git2SemVer.Tool.Commands.Changelog;

[RegisterSingleton]
internal sealed class ChangelogCommand(IConsoleIO console) : CommandBase(console), IChangelogCommand
{
    public void Execute(ChangelogCommandSettings cmdLineSettings)
    {
        try
        {
            Console.WriteMarkupInfoLine($"Generating Changelog {(cmdLineSettings.Unattended ? " (unattended)" : "")}.");
            Console.WriteLine("");

            var proceed = Console.PromptYesNo("Proceed?");
            Console.WriteLine();
            if (!proceed)
            {
                Console.WriteErrorLine("Aborted.");
            }

            var stopwatch = Stopwatch.StartNew();

            EnsureDataDirectoryExists(cmdLineSettings);
            var projectSettings = GetProjectSettings(cmdLineSettings);
            
            var changeLogInputs = RunVersionGenerator(cmdLineSettings, projectSettings);

            changeLogInputs.Save(Path.Combine(cmdLineSettings.DataDirectory, "test.json")); // >>> test

            var outputFileExists = File.Exists(cmdLineSettings.OutputFilePath);
            var createNewChangelog = !outputFileExists || !cmdLineSettings.Incremental;
            var lastRunData = GetLastRunData(cmdLineSettings, createNewChangelog);
            if (!createNewChangelog && !projectSettings.AllowVariationsToSemVerStandard)
            {
                if (lastRunData.ContributingReleasesChanged(changeLogInputs.ContribReleases))
                {
                    Console.WriteMarkupInfoLine("[lightsalmon1]There has been a release since last run, a new changelog will be generated.[/]");
                    lastRunData = new LastRunData();
                    createNewChangelog = true;
                }
            }

            var canProceed = AskIfToProceed(cmdLineSettings, outputFileExists, changeLogInputs.HeadCommitSha, lastRunData);
            if (!canProceed)
            {
                Console.WriteLine();
                Console.WriteMarkupInfoLine("[em]Aborted[/]");
                return;
            }

            var changelog = Generate(cmdLineSettings, projectSettings, createNewChangelog, changeLogInputs, lastRunData);

            if (cmdLineSettings.WriteToConsole)
            {
                Console.WriteLine($"\n{(createNewChangelog ? "Created" : "Updated")} changelog:");
                Console.WriteHorizontalLine();
                Console.WriteCodeLine(changelog.TrimEnd());
                Console.WriteHorizontalLine();
            }

            if (cmdLineSettings.OutputFilePath.Length == 0)
            {
                Console.WriteLine();
                Console.WriteMarkupDebugLine("Write changelog to file is disabled as the file output path is an empty string.");
                return;
            }

            lastRunData.Update(changeLogInputs);
            lastRunData.Save(cmdLineSettings.DataDirectory, cmdLineSettings.OutputFilePath);

            Console.WriteLine();
            var verb = cmdLineSettings.Incremental && !createNewChangelog ? "Updating" :
                !cmdLineSettings.Incremental && createNewChangelog ? "Overwriting" :
                "Creating";
            Console.WriteMarkupInfoLine($"{verb} changelog file: {cmdLineSettings.OutputFilePath}");
            File.WriteAllText(cmdLineSettings.OutputFilePath, changelog);

            projectSettings.Save(Path.Combine(cmdLineSettings.DataDirectory, ChangelogResources.ProjectSettingsFilename));

            stopwatch.Stop();

            Console.WriteLine("");
            Console.WriteMarkupLine($"[good]Completed[/] (in {stopwatch.ElapsedMilliseconds:D0} ms)");
        }
        catch (Exception exception)
        {
            Console.WriteErrorLine(exception);
            throw;
        }
    }

    private ChangelogInputs RunVersionGenerator(ChangelogCommandSettings cmdLineSettings, ChangelogLocalSettings projectSettings)
    {
        var inputs = new VersionGeneratorInputs
        {
            VersioningMode = VersioningMode.StandAloneProject,
            IntermediateOutputDirectory = cmdLineSettings.DataDirectory,
            HostType = cmdLineSettings.HostType ?? ""
        };

        using var logger = CreateLogger(cmdLineSettings.Verbosity);
        var host = GetBuildHost(logger, inputs);
        var versionGenerator = new VersionGeneratorFactory(logger).Create(inputs,
                                                                          new NullMSBuildGlobalProperties(),
                                                                          new NullJsonFileIO(),
                                                                          host,
                                                                          projectSettings.ConvCommits);
        var (outputs, contributing) = versionGenerator.CalculateSemanticVersion();
        return new ChangelogInputs(outputs, contributing);
    }

    private bool AskIfToProceed(ChangelogCommandSettings commandSettings,
                                bool outputFileExists,
                                string headSha,
                                LastRunData lastRunData)
    {
        if (commandSettings.Force || !outputFileExists || !string.Equals(headSha, lastRunData.HeadSha))
        {
            return true;
        }

        Console.WriteMarkupInfoLine("The changelog exists and the head commit has not changed since last run. There should be no changes.");

        if (commandSettings.Unattended)
        {
            return false;
        }

        return Console.PromptYesNo($"{(commandSettings.Incremental ? "Update" : "Recreate")} anyway?", false);
    }

    private static void EnsureDataDirectoryExists(ChangelogCommandSettings cmdLineSettings)
    {
        var dataDirectory = cmdLineSettings.DataDirectory;
        // ReSharper disable once InvertIf
        if (dataDirectory.Length > 0)
        {
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }
        }
    }

    private string Generate(ChangelogCommandSettings commandSettings,
                            ChangelogLocalSettings projectSettings,
                            bool createNewChangelog,
                            ChangelogInputs inputs,
                            LastRunData lastRunData)
    {
        var template = GetTemplate(commandSettings.DataDirectory);
        var releaseUrl = commandSettings.ArtifactLinkPattern;
        var changelogGenerator = new ChangelogGenerator(projectSettings);

        var existingChangelog = createNewChangelog ? "" : File.ReadAllText(commandSettings.OutputFilePath);
        var changelog = changelogGenerator.Execute(inputs,
                                                   lastRunData,
                                                   template,
                                                   existingChangelog, releaseUrl, true, createNewChangelog);

        if (string.Equals(existingChangelog, changelog))
        {
            Console.WriteMarkupInfoLine("No updates found.");
        }

        return changelog;
    }

    private static LastRunData GetLastRunData(ChangelogCommandSettings cmdLineSettings, bool reset)
    {
        if (reset)
        {
            return new LastRunData();
        }

        EnsureDataDirectoryExists(cmdLineSettings);
        return LastRunData.Load(cmdLineSettings.DataDirectory, cmdLineSettings.OutputFilePath);
    }

    private static ChangelogLocalSettings GetProjectSettings(ChangelogCommandSettings commandSettings)
    {
        var filePath = Path.Combine(commandSettings.DataDirectory, ChangelogResources.ProjectSettingsFilename);

        if (File.Exists(filePath))
        {
            return ChangelogLocalSettings.Load(filePath);
        }

        var config = new ChangelogLocalSettings
        {
            Categories = ChangelogResources.DefaultCategories
        };
        config.Save(filePath);
        return config;
    }

    private string GetTemplate(string directory)
    {
        var templatePath = Path.Combine(directory, ChangelogResources.DefaultMarkdownTemplateFilename);
        if (File.Exists(templatePath))
        {
            return File.ReadAllText(templatePath);
        }

        Console.WriteMarkupDebugLine($"Creating default template file: {templatePath}");
        var defaultTemplate = ChangelogResources.GetDefaultTemplate();
        File.WriteAllText(templatePath, defaultTemplate);
        return defaultTemplate;
    }
}