using System.Text;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools;


namespace NoeticTools.Git2SemVer.Tool.Integration.Tests.Framework;

public static class DotNetProcessHelpers
{
    public static string GetSolutionDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "../../../..")).FullName;
        return solutionDirectory;
    }

    //public static void Publish(string projectPath, string buildConfiguration)
    //{
    //    TestContext.Out.WriteLine($"\nPublishing {projectPath}\n");
    //    const string profileFile = "FolderProfile.pubxml";
    //    var dotNetCommandLine = $"publish {projectPath} /p:PublishProfile={profileFile} --configuration {buildConfiguration} --no-build --no-restore";
    //    var result = new DotNetTool().RunReturningStdOut(dotNetCommandLine, TestContext.Out, TestContext.Error);
    //    Assert.That(result, Is.EqualTo(0), $"Command failed: dotnet {dotNetCommandLine}");
    //}

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