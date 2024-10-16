using Moq;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


//#pragma warning disable NUnit2045


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

[TestFixture]
internal class PathsFromLastReleasesFinderTests : GitHistoryWalkingTestsBase
{
    private PathsFromLastReleasesFinder _target;
    private Mock<IGitTool> _gitTool;

    [SetUp]
    public void Setup()
    {
        base.SetupBase();

        _gitTool = new Mock<IGitTool>();

        _target = new PathsFromLastReleasesFinder(_repository.Object, _gitTool.Object, _logger);

        _gitTool.Setup(x => x.BranchName).Returns("BranchName");
    }

    [TearDown]
    public void TearDown()
    {
        _logger.Dispose();
    }


    [TestCaseSource(typeof(ScenariosFromBuildLogs), nameof(ScenariosFromBuildLogs.Scenarios))]
    public void BasicScenariosTest(LoggedScenario scenario)
    {
        var commits = SetupGitRepository(scenario.ActualGitLog);

        var paths = _target.FindPathsToHead();

        var bestPath = paths.BestPath;
        Assert.That(bestPath.Version.ToString(), Is.EqualTo(scenario.ExpectedVersion));
    }
}

