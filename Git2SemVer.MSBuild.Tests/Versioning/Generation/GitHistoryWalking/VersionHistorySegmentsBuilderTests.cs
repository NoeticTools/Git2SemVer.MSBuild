using Moq;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;


//#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

[TestFixture]
internal class VersionHistorySegmentsBuilderTests : GitHistoryWalkingTestsBase
{
    private Mock<IGitTool> _gitTool;
    private VersionHistorySegmentsBuilder _target;

    [SetUp]
    public void Setup()
    {
        SetupBase();

        _gitTool = new Mock<IGitTool>();

        _target = new VersionHistorySegmentsBuilder(Repository.Object, Logger);

        _gitTool.Setup(x => x.BranchName).Returns("BranchName");
    }

    [TearDown]
    public void TearDown()
    {
        Logger.Dispose();
    }

    [TestCaseSource(typeof(ScenariosFromBuildLogsTestSource))]
    public void BasicScenariosTest(string name, LoggedScenario scenario)
    {
        var commits = SetupGitRepository(scenario);

        var segments = _target.BuildTo(commits["0001"]);

        Assert.That(segments, Is.Not.Null);
    }

    [TestCase]
    public void DetailedScenario01SegmentsTest()
    {
        var scenario = new ScenariosFromBuildLogsTestSource().Scenario01;
        var commits = SetupGitRepository(scenario);

        var segments = _target.BuildTo(commits[scenario.HeadCommitId]);

        Assert.That(segments, Is.Not.Null);
        Assert.That(segments, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            var segment = segments[0];
            Assert.That(segment.Id, Is.EqualTo(1));
            Assert.That(segment.Commits, Has.Count.EqualTo(1));
            Assert.That(segment.FirstCommit.CommitId.ObfuscatedSha, Is.EqualTo("0001"));
            Assert.That(segment.LastCommit.CommitId.ObfuscatedSha, Is.EqualTo("0001"));
            Assert.That(segment.To, Has.Count.EqualTo(0));
            Assert.That(segment.From, Has.Count.EqualTo(2));
            Assert.That(segment.TaggedReleasedVersion, Is.Null);
        });
        Assert.That(segments[1].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[2].Commits, Has.Count.EqualTo(2));
        Assert.That(segments[3].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[4].Commits, Has.Count.EqualTo(1));
        Assert.That(segments[5].Commits, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            var segment = segments[6];
            Assert.That(segment.Commits, Has.Count.EqualTo(3));
            Assert.That(segment.To, Has.Count.EqualTo(2));
            Assert.That(segment.To[0].Id, Is.EqualTo(5));
            Assert.That(segment.To[1].Id, Is.EqualTo(6));
            Assert.That(segment.TaggedReleasedVersion!.ToString(), Is.EqualTo("0.3.1"));
        });
        Assert.That(segments[7].Commits, Has.Count.EqualTo(1));
    }
}