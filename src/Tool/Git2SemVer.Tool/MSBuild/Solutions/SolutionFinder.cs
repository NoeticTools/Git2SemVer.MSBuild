using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;


namespace NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;

[RegisterSingleton]
internal sealed class SolutionFinder(ILogger logger) : ISolutionFinder
{
    public FileInfo Find(string inputSolutionFile)
    {
        if (string.IsNullOrWhiteSpace(inputSolutionFile))
        {
            return Find(new DirectoryInfo(Environment.CurrentDirectory));
        }

        if (File.Exists(inputSolutionFile))
        {
            return new FileInfo(inputSolutionFile);
        }

        throw new Git2SemVerFileNotFoundException($"The solution '{inputSolutionFile}' was not found in the working directory.");
    }

    private FileInfo Find(DirectoryInfo solutionDirectory)
    {
        var solutions = solutionDirectory.GetFiles("*.sln");
        if (solutions.Length == 0)
        {
            throw new Git2SemVerFileNotFoundException($"Unable to find any solution (.sln) in the current directory '{solutionDirectory.FullName}'.");
        }

        if (solutions.Length > 1)
        {
            throw new
                Git2SemVerFileNotFoundException("More than one solution (.sln) in the current directory. Use --Solution option to select the solution..");
        }

        var solutionFile = solutions[0];
        logger.LogDebug($"Using solution {solutionFile.FullName}");
        return solutionFile;
    }
}