using System.ComponentModel;
using Spectre.Console.Cli;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global


namespace NoeticTools.Git2SemVer.Tool.CommandLine;

public class CommonCommandSettings : CommandSettings
{
    [CommandOption("-s|--solution")]
    [DefaultValue("")]
    [Description("Solution name. Optional, only required when there are multiple solutions in the working directory.")]
    public string SolutionName { get; set; } = "";

    [CommandOption("-u|--unattended")]
    [DefaultValue(false)]
    [Description("Unattened execution. Accepts all defaults.")]
    public bool Unattended { get; set; }
}