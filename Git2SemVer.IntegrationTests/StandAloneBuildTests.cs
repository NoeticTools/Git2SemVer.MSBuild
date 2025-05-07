#pragma warning disable NUnit2045

namespace Git2SemVer.IntTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
//[NonParallelizable]
internal class StandAloneBuildTests : VersioningBuildTestsBase
{
    [Test]
    public void PackWithForcingProps2ScriptTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties2.csx");

        var result = context.DotNetCli.Pack(context.TestSolutionPath, context.BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath}");
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);

        var output = RunCompiledApp(context);

        Assert.That(output, Does.Contain("""
                                         Assembly version:       21.22.23.0
                                         File version:           21.22.23.0
                                         Informational version:  21.22.23-beta
                                         Product version:        21.22.23-beta
                                         """));
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.1.0.0.nupkg");
    }

    [Test]
    public void BuildWithForcingProps1ScriptTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties1.csx");

        context.DotNetCli.Build(context.TestSolutionPath, context.BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath}");

        var output = RunCompiledApp(context);
        Assert.That(output, Contains.Substring("""
                                               Assembly version:       1.2.3.0
                                               File version:           4.5.6
                                               Informational version:  11.12.13-a-prerelease+metadata
                                               Product version:        11.12.13-a-prerelease+metadata
                                               """));
    }

    protected override VersioningBuildTestContext CreateTestContext()
    {
        return new VersioningBuildTestContext("StandAlone", "StandAloneTestSolution",
                                              "StandAloneVersioning.sln", "TestApplication");
    }
}