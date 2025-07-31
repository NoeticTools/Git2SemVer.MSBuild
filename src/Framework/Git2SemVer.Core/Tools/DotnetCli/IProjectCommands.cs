namespace NoeticTools.Git2SemVer.Core.Tools.DotnetCli;

public interface IProjectCommands
{
    void New(string template, string projectPath, string language = "C#");
    void NewClassLib(string projectPath, string language = "C#");
    void NewConsole(string projectPath, string language = "C#");
}