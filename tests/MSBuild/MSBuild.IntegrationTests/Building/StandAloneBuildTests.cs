using NoeticTools.Git2SemVer.IntegrationTests.Framework;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.IntegrationTests.Building;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class StandAloneBuildTests : VersioningBuildTestsBase
{
    [Test]
    public void BuildAndPackWithForcingProperties2ScriptTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties2.csx");

        var returnCode = context.DotNetCli.Pack(context.TestSolutionPath, context.BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath}");

        Assert.That(returnCode, Is.Zero);
        Assert.That(File.Exists(context.CompiledAppPath), Is.True, $"File '{context.CompiledAppPath}' does not exist after build and pack.");

        var output = DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, context.Logger);
        Assert.That(output, Does.Contain("""
                                         Assembly version:       21.22.23.0
                                         File version:           21.22.23.0
                                         Informational version:  21.22.23-beta
                                         Product version:        21.22.23-beta
                                         """));
        VersioningBuildTestContext.AssertFileExists(context.PackageOutputDir, "NoeticTools.TestApplication.1.0.0.nupkg");
    }

    [Test]
    public void BuildOnlyWithForcingProperties1ScriptTest()
    {
        using var context = CreateTestContext();

        var scriptPath = context.DeployScript("ForceProperties1.csx");

        context.DotNetCli.Build(context.TestSolutionPath, context.BuildConfiguration, $"-p:Git2SemVer_ScriptPath={scriptPath}");

        context.ShowVersioningReport();

        var output = DotNetProcessHelpers.RunDotnetApp(context.CompiledAppPath, context.Logger);
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