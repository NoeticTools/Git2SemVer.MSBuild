namespace NoeticTools.Git2SemVer.Core.Tools.DotnetCli;

public sealed class ProjectCommands : IProjectCommands
{
    private readonly IDotNetTool _inner;

    internal ProjectCommands(IDotNetTool inner)
    {
        _inner = inner;
    }

    public void New(string template, string projectPath, string language = "C#")
    {
        var commandLineArguments = $"new {template} --framework net8.0 --language {language} --output {projectPath}";
        _inner.Run(commandLineArguments);
    }

    public void NewClassLib(string projectPath, string language = "C#")
    {
        New("classlib", projectPath, language);
    }

    public void NewConsole(string projectPath, string language = "C#")
    {
        New("console", projectPath, language);
    }
}