namespace NoeticTools.Git2SemVer.Core.Tools.DotnetCli;

public interface IDotNetTool
{
    IProjectCommands Projects { get; }

    ISolutionCommands Solution { get; }

    /// <summary>
    ///     Run dotnet cli with provided command line arguments.
    /// </summary>
    int Run(string commandLineArguments);
}