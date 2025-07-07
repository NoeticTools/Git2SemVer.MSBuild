using System.ComponentModel;
using NoeticTools.Git2SemVer.Tool.CommandLine;
using Spectre.Console.Cli;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.Commands.Changelog;

public class ChangelogCommandSettings : CommonCommandSettings
{
    [CommandOption("-a|--artifact-url <URL>")]
    [DefaultValue("https://www.nuget.org/packages/user.project/%VERSION%")]
    [Description("Optional url to a version's artifacts. Must contain version placeholder '%VERSION%'.")]
    public string ArtifactUrl { get; set; } = "";

    [CommandOption("-m|--meta-directory <DIRECTORY>")]
    [DefaultValue(".changelog")]
    [Description("Directory in which to place the generators metadata file.")]
    public string DataDirectory { get; set; } = "";

    [CommandOption("--host-type <TYPE>")]
    [Description("Force the host type. Use for testing expected behaviour on other hosts. Valid values are 'Custom', 'Uncontrolled', 'TeamCity', or 'GitHub'.")]
    public string? HostType { get; set; } = null;

    [CommandOption("-o|--output <FILEPATH>")]
    [DefaultValue("CHANGELOG.md")]
    [Description("Generated changelog file path. May be a relative or absolute path. Set to empty string to disable file write.")]
    public string OutputFilePath { get; set; } = "";

    [CommandOption("-v|--verbosity <LEVEL>")]
    [DefaultValue("info")]
    [Description("Sets output verbosity. Valid values are 'trace', 'debug', 'info', 'warning', or 'error'.")]
    public string Verbosity { get; set; } = "";

    [CommandOption("-c|--console-out")]
    [DefaultValue(false)]
    [Description("Enable writing generated changelog to the console.")]
    public bool WriteToConsole { get; set; }

    [CommandOption("-i|--incremental")]
    [DefaultValue(true)]
    [Description("Enable incremental changelog updating. Setting this to false will cause the changelog to be overwritten.")]
    public bool Incremental { get; set; }

    [CommandOption("-f|--force")]
    [DefaultValue(false)]
    [Description("Force create or update regardless if there a no new commits. Helpful for debugging.")]
    public bool Force { get; set; }
}