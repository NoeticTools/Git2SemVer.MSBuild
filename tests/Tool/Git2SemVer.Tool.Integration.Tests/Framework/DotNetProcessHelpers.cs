using System.Text;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Tool.Integration.Tests.Framework;

public static class DotNetProcessHelpers
{
    public static string GetSolutionDirectory()
    {
        return GitTool.DiscoverRepositoryDirectory(Directory.GetCurrentDirectory());
    }

    public static string RunDotnetApp(string appDllPath, ILogger logger)
    {
        logger.LogInfo($"Running '{appDllPath}'");
        var process = new ProcessCli(logger)
        {
            WorkingDirectory = Path.GetDirectoryName(appDllPath)!
        };
        var outputStringBuilder = new StringBuilder();
        var outputWriter = new StringWriter(outputStringBuilder);
        var returnCode = process.Run("dotnet", appDllPath, outputWriter, TestContext.Error);
        var output = outputStringBuilder.ToString();
        Assert.That(returnCode, Is.EqualTo(0));
        TestContext.Out.WriteLine();
        return output;
    }
}