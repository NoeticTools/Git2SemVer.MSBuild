using System.Diagnostics;
using System.Xml.Linq;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Tool.Commands.Run;


namespace NoeticTools.Git2SemVer.Tool.Commands.Changelog;

[RegisterSingleton]
internal sealed class ChangelogCommand(IConsoleIO console) : CommandBase(console), IChangelogCommand
{
    private readonly IConsoleIO _console = console;

    private const string LocalSettingsFilename = "changelog.settings.json";

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

            var inputs = new GeneratorInputs
            {
                VersioningMode = VersioningMode.StandAloneProject,
                IntermediateOutputDirectory = cmdLineSettings.DataDirectory,
                HostType = cmdLineSettings.HostType ?? ""
            };

            EnsureDataDirectoryExists(cmdLineSettings);
            var outputFileExists = File.Exists(cmdLineSettings.OutputFilePath);
            var createNewChangelog = !outputFileExists || !cmdLineSettings.Incremental;
            using var logger = CreateLogger(cmdLineSettings.Verbosity);
            var lastRunData = GetLastRunData(cmdLineSettings, createNewChangelog);
            var host = GetBuildHost(logger, inputs);
            var localSettings = GetLocalSettings(cmdLineSettings);
            var versionGenerator = new VersionGeneratorFactory(logger).Create(inputs,
                                                                              new NullMSBuildGlobalProperties(),
                                                                              new NullJsonFileIO(),
                                                                              host,
                                                                              localSettings.ConvCommits);

            var (outputs, contributing) = versionGenerator.CalculateSemanticVersion();


            var canProceed = CanProceed(cmdLineSettings, outputFileExists, contributing.Head.CommitId.Sha, lastRunData);
            if (!canProceed)
            {
                Console.WriteLine();
                Console.WriteMarkupInfoLine("[em]Aborted[/]");
                return;
            }

            var changelog = Generate(cmdLineSettings, localSettings, createNewChangelog, outputs, contributing, lastRunData);

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

            lastRunData.Update(outputs);
            Save(lastRunData, cmdLineSettings);

            Console.WriteLine();
            var verb = cmdLineSettings.Incremental && outputFileExists ? "Updating" :
                !cmdLineSettings.Incremental && outputFileExists ? "Overwriting" : "Creating";
            Console.WriteMarkupInfoLine($"{verb} changelog file: {cmdLineSettings.OutputFilePath}");
            File.WriteAllText(cmdLineSettings.OutputFilePath, changelog);

            localSettings.Save(Path.Combine(cmdLineSettings.DataDirectory, LocalSettingsFilename));

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

    private string Generate(ChangelogCommandSettings commandSettings,
                            ChangelogLocalSettings config,
                            bool createNewChangelog,
                            VersionOutputs outputs,
                            ContributingCommits contributing, 
                            LastRunData lastRunData)
    {
        var template = GetTemplate(commandSettings);
        var releaseUrl = commandSettings.ArtifactUrl;
        var changelogGenerator = new ChangelogGenerator(config);
        if (createNewChangelog)
        {
            return changelogGenerator.Create(releaseUrl,
                                             outputs,
                                             contributing,
                                             template,
                                             incremental: commandSettings.Incremental,
                                             lastRunData);
        }

        var existingChangelog = File.ReadAllText(commandSettings.OutputFilePath);
        var changelog = changelogGenerator.Update(releaseUrl,
                                                  outputs,
                                                  contributing,
                                                  template,
                                                  existingChangelog,
                                                  lastRunData);
        if (string.Equals(existingChangelog, changelog))
        {
            Console.WriteMarkupInfoLine("No updates found.");
        }
        return changelog;
    }

    private static void Save(LastRunData lastRunData, ChangelogCommandSettings cmdLineSettings)
    {
        lastRunData.Save(LastRunData.GetFilePath(cmdLineSettings.DataDirectory, cmdLineSettings.OutputFilePath));
    }

    private bool CanProceed(ChangelogCommandSettings commandSettings, 
                            bool outputFileExists, 
                            string headSha,
                            LastRunData lastRunData)
    {
        if (commandSettings.Force)
        {
            return true;
        }

        if (!outputFileExists || !string.Equals(headSha, lastRunData.HeadSha))
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

    private static LastRunData GetLastRunData(ChangelogCommandSettings cmdLineSettings, bool reset)
    {
        if (reset)
        {
            return new LastRunData();
        }
        EnsureDataDirectoryExists(cmdLineSettings);
        return LastRunData.Load(LastRunData.GetFilePath(cmdLineSettings.DataDirectory, cmdLineSettings.OutputFilePath));
    }

    private static ChangelogLocalSettings GetLocalSettings(ChangelogCommandSettings commandSettings)
    {
        var filePath = Path.Combine(commandSettings.DataDirectory, LocalSettingsFilename);

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

    private string GetTemplate(ChangelogCommandSettings settings)
    {
        var dataDirectory = settings.DataDirectory;

        var templatePath = Path.Combine(dataDirectory, ChangelogResources.DefaultMarkdownTemplateFilename);
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