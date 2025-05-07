using NoeticTools.Git2SemVer.IntegrationTests.Framework;


namespace NoeticTools.Git2SemVer.IntegrationTests;

internal abstract class VersioningBuildTestsBase
{
    [Test]
    [CancelAfter(60000)]
    public void BuildAndPackTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties3.csx");
        context.DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");
        context.PackTestSolution();
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.1.2.3-alpha.nupkg");

        var output = RunCompiledApp(context);
        Assert.That(output, Does.Contain("""
                                         Assembly version:       200.201.202.0
                                         File version:           200.201.212
                                         Informational version:  2.2.2-beta
                                         Product version:        2.2.2-beta
                                         """));
    }

    [Test]
    [CancelAfter(60000)]
    public void BuildOnlyTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties3.csx");
        context.DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");

        var output = RunCompiledApp(context);
        Assert.That(output, Does.Contain("""
                                         Assembly version:       200.201.202.0
                                         File version:           200.201.212
                                         Informational version:  2.2.2-beta
                                         Product version:        2.2.2-beta
                                         """));
    }

    [Test]
    [CancelAfter(60000)]
    public void PackTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties1.csx");

        var result = context.DotNetCli.Pack(context.TestSolutionPath, context.BuildConfiguration,
                                            $"-p:Git2SemVer_ScriptPath={scriptPath} -fileLogger");
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);

        var output = RunCompiledApp(context);
        Assert.That(output, Contains.Substring("""
                                               Assembly version:       1.2.3.0
                                               File version:           4.5.6
                                               Informational version:  11.12.13-a-prerelease+metadata
                                               Product version:        11.12.13-a-prerelease+metadata
                                               """));
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.5.6.7.nupkg");
    }

    protected abstract VersioningBuildTestContext CreateTestContext();

    protected static string RunCompiledApp(VersioningBuildTestContext context)
    {
        Assert.That(File.Exists(context.CompiledAppPath), Is.True, $"File '{context.CompiledAppPath}' does not exist.");
        var outputFilePath = Path.Combine(context.TestDirectory.FullName, "output.txt");
        DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, outputFilePath, context.Logger);
        return File.ReadAllText(outputFilePath);
    }
}