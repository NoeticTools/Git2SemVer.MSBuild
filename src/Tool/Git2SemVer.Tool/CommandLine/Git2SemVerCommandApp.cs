using Microsoft.Extensions.DependencyInjection;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Tool.CommandLine.Changelog;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.CommandLine.Versioning.Run;
using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

internal class Git2SemVerCommandApp
{
    public static int Execute(string[] args)
    {
        using var logger = new FileLogger(GetLogFilePath());
        return Execute(args, logger);
    }

    public static int Execute(string[] args, ILogger logger)
    {
        var servicesProvider = Services.ConfigureServices(logger);

        var app = new CommandApp();

        app.Configure(config =>
        {
            config.PropagateExceptions();

            config.SetApplicationName("git2semver");
            config.SetApplicationVersion(typeof(Git2SemVerCommandApp).Assembly.GetInformationalVersion());

            config.AddBranch<CommandSettings>("changelog", changelogBranch =>
            {
                changelogBranch.SetDescription("Changelog commands");

                changelogBranch.AddCommand<ChangelogCliCommand>("run")
                               .WithDescription("Generate/update changelog command")
                               .WithExample("changelog", "run")
                               .WithData(servicesProvider);
            });

            config.AddBranch<CommandSettings>("versioning", verBranch =>
            {
                verBranch.SetDescription("Solution versioning commands (alias 'ver')");

                verBranch.AddCommand<RunCliCommand>("run")
                         .WithDescription("Run version generator command")
                         .WithExample("versioning", "run")
                         .WithData(servicesProvider);

                verBranch.AddBranch<CommandSettings>("setup", bootBranch =>
                {
                    bootBranch.SetDescription("Solution versioning setup command (Alias 'setup')");

                    bootBranch.AddCommand<AddCliCommand>("add")
                              .WithDescription("Add Git2SemVer solution versioning to solution in working directory")
                              .WithData(servicesProvider)
                              .WithExample("versioning", "setup", "add")
                              .WithExample("versioning", "setup", "add", "-confirm", "false", "--solution", "'MyOtherSolution.sln'");
                    bootBranch.AddCommand<RemoveCliCommand>("remove")
                              .WithDescription("Remove Git2SemVer solution versioning from solution in working directory")
                              .WithData(servicesProvider)
                              .WithExample("versioning", "setup", "remove", "--solution", "'MyOtherSolution.sln'");

                });
            }).WithAlias("ver");

            // Depreciated
            config.AddCommand<AddCliCommand>("add")
                  .IsHidden()
                  .WithDescription("Add Git2SemVer solution versioning to solution in working directory")
                  .WithData(servicesProvider);

            // Depreciated
            config.AddCommand<RemoveCliCommand>("remove")
                  .IsHidden()
                  .WithDescription("Remove Git2SemVer solution versioning from solution in working directory")
                  .WithData(servicesProvider);

            // Depreciated
            config.AddCommand<RunCliCommand>("run")
                  .IsHidden()
                  .WithDescription("Run version generator")
                  .WithExample("versioning", "run")
                  .WithData(servicesProvider);
        });

        try
        {
            return app.Run(args);
        }
#pragma warning disable CA1031
        catch (Exception exception)
#pragma warning restore CA1031
        {
            var console = servicesProvider.GetService<IConsoleIO>()!;
            console.WriteErrorLine($"Error: {exception.Message}");
            logger.LogError(exception);
            return 100;
        }
    }

    private static string GetLogFilePath()
    {
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Git2SemVer");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, "Git2SemVer.Tool.log");
    }
}