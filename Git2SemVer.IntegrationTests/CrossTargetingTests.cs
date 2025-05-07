namespace Git2SemVer.IntTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
//[NonParallelizable]
internal class CrossTargetingTests : VersioningBuildTestsBase
{
    protected override VersioningBuildTestContext CreateTestContext()
    {
        return new VersioningBuildTestContext("CrossTarget", "CrossTargetingTestSolution", "CrossTargetingVersioning.sln", "TestApplication");
    }
}