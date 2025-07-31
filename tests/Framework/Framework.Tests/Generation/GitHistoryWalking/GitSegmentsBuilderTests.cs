using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;


//#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Framework.Tests.Generation.GitHistoryWalking;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class GitSegmentsBuilderTests
{
    [TestCaseSource(typeof(ScenariosFromBuildLogsTestSource))]
    public void BasicScenariosTest(string name, LoggedScenario scenario)
    {
        using var context = new GitHistoryWalkingTestsContext();
        context.SetupGitRepository(scenario);

        var target = new GitSegmentsBuilder(context.GitTool.Object, context.Logger);

        var segments = target.GetContributingCommits(context.GitTool.Object.Head);

        Assert.That(segments, Is.Not.Null);
    }

    [TestCase]
    public void DetailedScenario01SegmentsTest()
    {
        using var context = new GitHistoryWalkingTestsContext();
        var scenario = new ScenariosFromBuildLogsTestSource().Scenario01;
        context.SetupGitRepository(scenario);

        var target = new GitSegmentsBuilder(context.GitTool.Object, context.Logger);

        var contributingCommits = target.GetContributingCommits(context.GitTool.Object.Head);

        Assert.That(contributingCommits, Is.Not.Null);
        var segments = contributingCommits.Segments;
        Assert.That(segments, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            var segment = segments[0].Inner;
            Assert.That(segment.Id, Is.EqualTo(1));
            Assert.That(segment.Commits, Has.Count.EqualTo(1));
            Assert.That(segment.OldestCommit.CommitId.Sha, Is.EqualTo("0001"));
            Assert.That(segment.YoungestCommit.CommitId.Sha, Is.EqualTo("0001"));
            Assert.That(segment.ParentCommits, Has.Count.EqualTo(2));
            Assert.That(segment.Version, Is.Null);
        });
        Assert.That(segments[1].Commits, Has.Count.EqualTo(2));
        Assert.That(segments[2].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[3].Commits, Has.Count.EqualTo(3));
        Assert.That(segments[4].Commits, Has.Count.EqualTo(3));
        Assert.That(segments[4].Inner.Version!.ToString(), Is.EqualTo("0.3.1"));
        Assert.That(segments[5].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[6].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[7].Commits, Has.Count.EqualTo(1));
    }
}