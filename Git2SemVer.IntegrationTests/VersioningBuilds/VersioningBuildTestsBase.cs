using NoeticTools.Git2SemVer.IntegrationTests.Framework;


namespace NoeticTools.Git2SemVer.IntegrationTests.VersioningBuilds;

internal abstract class VersioningBuildTestsBase
{
    [Test]
    [CancelAfter(60000)]
    public void BuildAndThenPackWithoutRebuildTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties3.csx");
        context.DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");
        context.PackTestSolution();
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.1.2.3-alpha.nupkg");

        var output = DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, context.Logger);
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
        TestContext.Out.WriteLine("==101=="); //>>>
        TestContext.Out.Flush();
        using var context = CreateTestContext();

        TestContext.Out.WriteLine("==102=="); //>>>
        var scriptPath = context.DeployScript("ForceProperties3.csx");
        TestContext.Out.WriteLine("==103=="); //>>>
        TestContext.Out.Flush();
        context.DotNetCliBuildTestSolution($"-p:Git2SemVer_ScriptPath={scriptPath}");

        TestContext.Out.WriteLine("==104=="); //>>>
        TestContext.Out.Flush();
        var output = DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, context.Logger);
        TestContext.Out.WriteLine("==105=="); //>>>
        Assert.That(output, Does.Contain("""
                                         Assembly version:       200.201.202.0
                                         File version:           200.201.212
                                         Informational version:  2.2.2-beta
                                         Product version:        2.2.2-beta
                                         """));
    }

    [Test]
    [CancelAfter(60000)]
    public void PackWithForcingProperties1ScriptTest()
    {
        TestContext.Out.WriteLine("==01=="); //>>>
        using var context = CreateTestContext();

        TestContext.Out.WriteLine("==02=="); //>>>
        var scriptPath = context.DeployScript("ForceProperties1.csx");

        TestContext.Out.WriteLine("==03=="); //>>>
        var result = context.DotNetCli.Pack(context.TestSolutionPath, context.BuildConfiguration,
                                            $"-p:Git2SemVer_ScriptPath={scriptPath} -fileLogger");
        TestContext.Out.WriteLine("==04=="); //>>>
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);

        TestContext.Out.WriteLine("==05=="); //>>>
        var output = DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, context.Logger);
        TestContext.Out.WriteLine("==06=="); //>>>
        Assert.That(output, Contains.Substring("""
                                               Assembly version:       1.2.3.0
                                               File version:           4.5.6
                                               Informational version:  11.12.13-a-prerelease+metadata
                                               Product version:        11.12.13-a-prerelease+metadata
                                               """));
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.5.6.7.nupkg");
    }

    protected abstract VersioningBuildTestContext CreateTestContext();
}