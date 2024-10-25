using NoeticTools.Git2SemVer.IntegrationTests.Framework;
using NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.IntegrationTests;

[TestFixture]
internal class StandAloneVersioningBuildTests : SolutionTestsBase
{
    private string _outputAppPath;
    private string _packageOutputDir;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        OneTimeSetUpBase();

        //BuildGit2SemVerMSBuild();
        //BuildGit2SemVerTool();

        var testProjectBinDirectory = Path.Combine(TestSolutionDirectory, "TestApplication/bin/", BuildConfiguration);
        _outputAppPath = Path.Combine(testProjectBinDirectory, "net8.0", "NoeticTools.TestApplication.dll");
        _packageOutputDir = testProjectBinDirectory;
    }

    [SetUp]
    public void SetUp()
    {
        SetUpBase();
        if (Directory.Exists(_packageOutputDir))
        {
            Directory.Delete(_packageOutputDir, true);
        }
    }

    [Test]
    public void BuildAndPackWithForcingProperties2ScriptTest()
    {
        var scriptPath = DeployScript("ForceProperties2.csx");

        var result = DotNetCli.Pack(TestSolutionPath, BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath} --verbosity normal");
        TestContext.Progress.WriteLine(result.stdOutput);
        Assert.That(File.Exists(_outputAppPath), Is.True, $"File '{_outputAppPath}' does not exist after build and pack.");
        Assert.That(result.returnCode, Is.EqualTo(0));

        var output = DotNetProcessHelpers.RunDotnetApp(_outputAppPath, Logger);

        Assert.That(output, Does.Contain("""
                                         Assembly version:       21.22.23.0
                                         File version:           21.22.23.0
                                         Informational version:  21.22.23-beta
                                         Product version:        21.22.23-beta
                                         """));
        AssertFileExists(_packageOutputDir, "NoeticTools.TestApplication.1.0.0.nupkg");
    }

    [Test]
    public void BuildAndThenPackWithoutRebuildTest()
    {
        BuildTestSolutionAndRemovePackage("ForceProperties3.csx");
        PackTestSolution();

        var output = DotNetProcessHelpers.RunDotnetApp(_outputAppPath, Logger);
        Assert.That(output, Does.Contain("""
                                         Assembly version:       200.201.202.0
                                         File version:           200.201.212
                                         Informational version:  2.2.2-beta
                                         Product version:        2.2.2-beta
                                         """));
        AssertFileExists(_packageOutputDir, "NoeticTools.TestApplication.1.2.3-alpha.nupkg");
    }

    [Test]
    public void BuildOnlyTest()
    {
        var scriptPath = DeployScript("ForceProperties3.csx");
        DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");

        var output = DotNetProcessHelpers.RunDotnetApp(_outputAppPath, Logger);
        Assert.That(output, Does.Contain("""
                                         Assembly version:       200.201.202.0
                                         File version:           200.201.212
                                         Informational version:  2.2.2-beta
                                         Product version:        2.2.2-beta
                                         """));
    }

    [Test]
    public void BuildOnlyWithForcingProperties1ScriptTest()
    {
        var scriptPath = DeployScript("ForceProperties1.csx");

        DotNetCli.Build(TestSolutionPath, BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath}");

        var output = DotNetProcessHelpers.RunDotnetApp(_outputAppPath, Logger);
        Assert.That(output, Contains.Substring("""
                                               Assembly version:       1.2.3.0
                                               File version:           4.5.6
                                               Informational version:  11.12.13-a-prerelease+metadata
                                               Product version:        11.12.13-a-prerelease+metadata
                                               """));
    }

    [Test]
    public void CanBuildTestSolutionTest()
    {
        DotNetCliBuildTestSolution();
    }

    [Test]
    public void PackWithForcingProperties1ScriptTest()
    {
        var scriptPath = DeployScript("ForceProperties1.csx");

        DotNetCli.Pack(TestSolutionPath, BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath} -fileLogger");

        var output = DotNetProcessHelpers.RunDotnetApp(_outputAppPath, Logger);
        Assert.That(output, Contains.Substring("""
                                               Assembly version:       1.2.3.0
                                               File version:           4.5.6
                                               Informational version:  11.12.13-a-prerelease+metadata
                                               Product version:        11.12.13-a-prerelease+metadata
                                               """));
        AssertFileExists(_packageOutputDir, "NoeticTools.TestApplication.4.6.7.nupkg");
    }

    private void DotNetCliBuildTestSolution(params string[] arguments)
    {
        var result = DotNetCli.Build(TestSolutionPath, BuildConfiguration, arguments);
        TestContext.Progress.WriteLine(result.stdOutput);
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);
        Assert.That(Logger.HasError, Is.False);
    }

    private void PackTestSolution()
    {
        var result = DotNetCli.Pack(TestSolutionPath, BuildConfiguration, "--no-restore --no-build");
        TestContext.Progress.WriteLine(result.stdOutput);
        Assert.That(result.returnCode, Is.EqualTo(0));
        Assert.That(Logger.HasError, Is.False);
    }

    private void BuildTestSolutionAndRemovePackage(string scriptName)
    {
        var scriptPath = DeployScript(scriptName);
        DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");
        DeleteAllNuGetPackages(_packageOutputDir);
    }

    private void BuildTestSolutionAndRemovePackage()
    {
        DotNetCliBuildTestSolution();
        DeleteAllNuGetPackages(_packageOutputDir);
    }

    protected override string SolutionFolderName => "StandAloneVersioning";

    protected override string SolutionName => "Git2SemVerTestApplication.sln";

    private static void AssertFileExists(string packageDirectory, string expectedFilename)
    {
        var directory = new DirectoryInfo(packageDirectory);
        var foundFiles = directory.GetFiles(expectedFilename);
        Assert.That(foundFiles.Length, Is.EqualTo(1), $"File '{expectedFilename}' does not exist.");
    }
}