using NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;


//#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Framework.Tests.Versioning.GitHistoryWalking;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class GitHistoryWalkerTests
{
    [TestCaseSource(typeof(ScenariosFromBuildLogsTestSource))]
    public void BasicScenariosTest(string name, LoggedScenario scenario)
    {
        using var context = new GitHistoryWalkingTestsContext();
        context.SetupGitRepository(scenario);
        var target = new GitHistoryWalker(context.GitTool.Object, context.Logger);

        var result = target.CalculateSemanticVersion();

        Assert.That(result.Version.ToString(), Is.EqualTo(scenario.ExpectedVersion));
    }
}