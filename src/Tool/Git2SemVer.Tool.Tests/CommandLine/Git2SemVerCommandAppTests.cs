using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Tool.CommandLine;


namespace NoeticTools.Git2SemVer.Tool.Tests.CommandLine;

[TestFixture]
[NonParallelizable]
internal class Git2SemVerCommandAppTests
{
    [TestCase("changelog", "run", "-c", "false", "--xxx", "0")]
    [TestCase("versioning", "run", "--host-type", "xxx")]
    [TestCase("versioning", "-c", "false", "run", "--xxx")]
    [TestCase("versioning", "setup", "add", "--solution", "xxx")]
    public void WithInvalidArgumentReturnsNonZeroExitCodeTest(params string[] args)
    {
        var stringLogger = new StringLogger();

        var exitCode = Git2SemVerCommandApp.Execute(args, stringLogger);

        Assert.That(exitCode, Is.Not.Zero);
        Assert.That(stringLogger.HasError);
    }

    [Test]
    public void WithoutArgumentsReturnsNonZeroExitCodeTest()
    {
        var exitCode = Git2SemVerCommandApp.Execute([""]);

        Assert.That(exitCode, Is.Not.Zero);
    }

    [Test]
    public void WithVersionOptionsShowsToolInfoVersionTest()
    {
        var exitCode = Git2SemVerCommandApp.Execute(["--version"]);
        // Spectre.Console.Cli handles this straight to console

        Assert.That(exitCode, Is.Zero);
    }
}