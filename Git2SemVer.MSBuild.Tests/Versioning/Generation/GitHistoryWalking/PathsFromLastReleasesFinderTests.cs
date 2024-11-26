using Moq;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


//#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class PathsFromLastReleasesFinderTests
{
    [TestCaseSource(typeof(ScenariosFromBuildLogsTestSource))]
    public void BasicScenariosTest(string name, LoggedScenario scenario)
    {
        using var context = new GitHistoryWalkingTestsContext();

        var target = new PathsFromLastReleasesFinder(context.Repository.Object, context.GitTool.Object, context.Logger);

        var commits = context.SetupGitRepository(scenario);

        var paths = target.FindPathsToHead();

        var bestPath = paths.BestPath;
        Assert.That(bestPath.Version.ToString(), Is.EqualTo(scenario.ExpectedVersion));
    }
}