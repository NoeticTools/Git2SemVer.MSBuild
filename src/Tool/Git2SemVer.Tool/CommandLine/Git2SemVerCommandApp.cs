using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;
using NoeticTools.Git2SemVer.Tool.Commands.Changelog;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Add;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Remove;
using NoeticTools.Git2SemVer.Tool.Commands.Versioning.Run;
using Spectre.Console.Cli;


// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

internal class Git2SemVerCommandApp
{
    public static int Execute(string[] args)
    {
        using var logger = new FileLogger(GetLogFilePath());
        var servicesProvider = Services.ConfigureServices(logger);

        var app = new CommandApp();

        app.Configure(config =>
        {
            config.SetApplicationName("dotnet git2semver");
            config.UseAssemblyInformationalVersion();

            config.AddBranch<CommandSettings>("versioning", branch =>
            {
                branch.SetDescription("Solution versioning commands (Alias 'ver')");

                branch.AddCommand<RunCliCommand>("run")
                      .WithDescription("Run version generator.")
                      .WithData(servicesProvider);

                branch.AddBranch<CommandSettings>("solution-setup", bootBranch =>
                {
                    bootBranch.SetDescription("Solution versioning setup commands (Alias 'setup')");

                    bootBranch.AddCommand<AddCliCommand>("add")
                              .WithDescription("Add Git2SemVer solution versioning to solution in working directory")
                              .WithData(servicesProvider)
                              .WithExample("versioning", "solution-setup", "add ")
                              .WithExample("ver", "setup", "add")
                              .WithExample("ver", "setup", "add -u")
                              .WithExample("ver", "setup", "add -u", "--solution", "'MyOtherSolution.sln'");
                    bootBranch.AddCommand<RemoveCliCommand>("remove")
                              .WithDescription("Remove Git2SemVer solution versioning from solution in working directory")
                              .WithData(servicesProvider)
                              .WithExample("versioning", "install", "remove", "--solution", "'MyOtherSolution.sln'");
                }).WithAlias("setup");
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
                  .WithData(servicesProvider);

            config.AddCommand<ChangelogCliCommand>(ChangelogConstants.DefaultSubfolderName)
                  .WithDescription("Generate changelog command.")
                  .WithData(servicesProvider);
        });

        return app.Run(args);
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