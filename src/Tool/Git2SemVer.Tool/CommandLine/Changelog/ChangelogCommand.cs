using System.Diagnostics;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Diagnostics;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;


namespace NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;

[RegisterSingleton]
internal sealed class ChangelogCommand(IConsoleIO console, ILogger logger) : CommandBase(console), IChangelogCommand
{
    public void Execute(ChangelogCommandSettings cmdLineSettings)
    {
        var proceed = WriteConsolePreamble(cmdLineSettings);
        if (!proceed)
        {
            Console.WriteErrorLine("Aborted.");
        }

        var stopwatch = Stopwatch.StartNew();

        var createNewChangelog = !File.Exists(cmdLineSettings.OutputFilePath);
        var priorChangelog = createNewChangelog ? "" : File.ReadAllText(cmdLineSettings.OutputFilePath);
        var projectSettings = ChangelogProjectSettings.Load(cmdLineSettings.DataDirectory, ChangelogConstants.ProjectSettingsFilename);
        var changeLogInputs = RunVersionGenerator(cmdLineSettings, projectSettings.ConvCommits);

        var changelogGenerator = new ChangelogGenerator(projectSettings, logger);
        var changelog = changelogGenerator.Execute(changeLogInputs,
                                                   cmdLineSettings.ArtifactLinkPattern,
                                                   cmdLineSettings.ReleaseAs,
                                                   cmdLineSettings.DataDirectory, 
                                                   cmdLineSettings.OutputFilePath);

        if (string.Equals(priorChangelog, changelog, StringComparison.Ordinal))
        {
            Console.WriteMarkupInfoLine("No updates found.");
        }

        var verb = !createNewChangelog ? "Updated" : "Created";
        if (cmdLineSettings.WriteToConsole)
        {
            Console.WriteLine($"\n{verb} changelog:");
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

        Console.WriteLine();
        Console.WriteMarkupInfoLine($"{verb} changelog file: {cmdLineSettings.OutputFilePath}");

        stopwatch.Stop();

        Console.WriteLine("");
        Console.WriteMarkupLine($"[good]Completed[/] (in {stopwatch.ElapsedMilliseconds:D0} ms)");
    }

    private ConventionalCommitsVersionInfo RunVersionGenerator(ChangelogCommandSettings cmdLineSettings, ConventionalCommitsSettings convCommits)
    {
        var inputs = new VersionGeneratorInputs
        {
            VersioningMode = VersioningMode.StandAloneProject,
            IntermediateOutputDirectory = cmdLineSettings.DataDirectory,
            HostType = cmdLineSettings.HostType ?? "",
            WriteConventionalCommitsInfo = false
        };

        using var versioningLogger = CreateLogger(cmdLineSettings.Verbosity);
        var host = GetBuildHost(versioningLogger, inputs);
        var versionGenerator = new VersioningEngineFactory(versioningLogger).Create(inputs,
                                                                          new NullMSBuildGlobalProperties(),
                                                                          new NullJsonFileIO(),
                                                                          host,
                                                                          convCommits);
        return versionGenerator.GetConventionalCommitsInfo();
    }

    private bool WriteConsolePreamble(ChangelogCommandSettings cmdLineSettings)
    {
        Console.WriteMarkupInfoLine($"Generating Changelog {(cmdLineSettings.Unattended ? " (unattended)" : "")}.");
        Console.WriteLine("");

        var proceed = Console.PromptYesNo("Proceed?");
        Console.WriteLine();
        return proceed;
    }
}