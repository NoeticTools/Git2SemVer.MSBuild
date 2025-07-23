using System.ComponentModel;
using Spectre.Console.Cli;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace NoeticTools.Git2SemVer.Tool.CommandLine;

public class CommonCommandSettings : CommandSettings
{
    [CommandOption("-c|--confirm")]
    [DefaultValue(true)]
    [Description("Ask user before operation is performed. If false (unattended mode) choices defaults are used.")]
    public bool Confirm { get; set; }
}