namespace NoeticTools.Git2SemVer.IntegrationTests.Building;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class CrossTargetingSolutionBuildTests : VersioningBuildTestsBase
{
    protected override VersioningBuildTestContext CreateTestContext()
    {
        return new VersioningBuildTestContext("CrossTarget", "CrossTargetingTestSolution", "CrossTargetingVersioning.sln", "TestApplication");
    }
}