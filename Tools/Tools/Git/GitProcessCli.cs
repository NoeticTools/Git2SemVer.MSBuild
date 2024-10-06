using Injectio.Attributes;
using NoeticTools.Common.Logging;


namespace NoeticTools.Common.Tools.Git;

[RegisterTransient]
public class GitProcessCli : IGitProcessCli
{
    private readonly IProcessCli _inner;
    private readonly string _gitPath;

    public GitProcessCli(ILogger logger)
    {
        _inner = new ProcessCli(logger);

        var teamCityGitPath = Environment.GetEnvironmentVariable("TEAMCITY_GIT_PATH") ?? "";
        _gitPath = teamCityGitPath.Length > 0 ? teamCityGitPath : "git";
        logger.LogDebug($"Using git path: '{_gitPath}'");
    }

    public string WorkingDirectory
    {
        get => _inner.WorkingDirectory;
        set => _inner.WorkingDirectory = value;
    }

    public int Run(string commandLineArguments,
                   TextWriter standardOut, TextWriter errorOut)
    {
        return _inner.Run(_gitPath, commandLineArguments, standardOut, errorOut);
    }
}