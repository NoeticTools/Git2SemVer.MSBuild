using NoeticTools.Git2SemVer.Core.Console;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;

[RegisterSingleton]
internal sealed class SolutionFinder : ISolutionFinder
{
    private readonly IConsoleIO _console;
    private readonly ILogger _logger;

    public SolutionFinder(IConsoleIO console, ILogger logger)
    {
        _console = console;
        _logger = logger;
    }

    public FileInfo? Find(string inputSolutionFile)
    {
        if (!string.IsNullOrWhiteSpace(inputSolutionFile))
        {
            if (File.Exists(inputSolutionFile))
            {
                return new FileInfo(inputSolutionFile);
            }

            throw new Git2SemVerArgumentException($"The solution '{inputSolutionFile}' was not found in the working directory.");
        }

        var solutionFile = Find(new DirectoryInfo(Environment.CurrentDirectory));
        return _logger.HasError ? null : solutionFile!;
    }

    private FileInfo? Find(DirectoryInfo solutionDirectory)
    {
        var solutions = solutionDirectory.GetFiles("*.sln");
        if (solutions.Length == 0)
        {
            _console.WriteErrorLine($"Unable to find any solution (.sln) in the current directory '{solutionDirectory.FullName}'.");
            return null;
        }

        if (solutions.Length > 1)
        {
            _console.WriteErrorLine("More than one solution (.sln) in the current directory. Use --Solution option to select the solution..");
            return null;
        }

        var solutionFile = solutions[0];
        _logger.LogDebug($"Using solution {solutionFile.FullName}");
        return solutionFile;
    }
}