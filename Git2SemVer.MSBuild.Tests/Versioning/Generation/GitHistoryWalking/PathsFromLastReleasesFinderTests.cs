﻿using Moq;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


//#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
internal class PathsFromLastReleasesFinderTests : GitHistoryWalkingTestsBase
{
    [SetUp]
    public void Setup()
    {
        SetupBase();
    }

    [TestCaseSource(typeof(ScenariosFromBuildLogsTestSource))]
    public void BasicScenariosTest(string name, LoggedScenario scenario)
    {
        var gitTool = new Mock<IGitTool>();
        var target = new PathsFromLastReleasesFinder(Repository.Object, gitTool.Object, Logger);
        gitTool.Setup(x => x.BranchName).Returns("BranchName");

        var commits = SetupGitRepository(scenario);

        var paths = target.FindPathsToHead();

        var bestPath = paths.BestPath;
        Assert.That(bestPath.Version.ToString(), Is.EqualTo(scenario.ExpectedVersion));
    }
}