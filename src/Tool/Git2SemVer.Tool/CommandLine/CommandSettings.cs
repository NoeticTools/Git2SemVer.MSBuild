using System.ComponentModel;
using Spectre.Console.Cli;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

public class CommonCommandSettings : CommandSettings
{
    [CommandOption("-u|--unattended")]
    [DefaultValue(false)]
    [Description("Unattened execution. Accepts all defaults.")]
    public bool Unattended { get; set; }
}