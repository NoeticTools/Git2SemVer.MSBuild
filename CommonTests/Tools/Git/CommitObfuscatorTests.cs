using NoeticTools.Common.ConventionCommits;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.CommonTests.Tools.Git;

internal class CommitObfuscatorTests
{
    [SetUp]
    public void SetUp()
    {
        CommitObfuscator.Clear();
    }

    [Test]
    public void NoConventionalCommitInfoLogLineTest()
    {
        const string expected = "*               \u001f.|0001|0002 0003|\u0002REDACTED\u0003|\u0002\u0003||\u001e";
        var commit = new Commit("commitSha", ["parent1", "parent2"], "Summary line", "", "", new CommitMessageMetadata());

        var result = CommitObfuscator.GetObfuscatedLogLine("* ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void WithConventionalCommitSummaryLogLineTest()
    {
        const string summary = "feat!: Big red feature";
        var expected = $"|\\              \u001f.|0001|0002 0003|\u0002{summary}\u0003|\u0002\u0003||\u001e";
        var commit = new Commit("commitSha", 
                                ["parent1", "parent2"], 
                                summary, "", "", 
                                new CommitMessageMetadata("feat", true, "Big red feature\nRecommended", "", []));

        var result = CommitObfuscator.GetObfuscatedLogLine(@"|\  ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void WithFooterValuesLogLineTest()
    {
        const string summary = "fix: Fixed";
        var footerKeyValues = new List<(string key, string value)>()
        {
            ("BREAKING CHANGE", "Oops my bad"),
            ("refs", "#0001"),
            ("refs", "#0002"),
        };
        var expected = $"|\\              \u001f.|0001|0002 0003|\u0002{summary}\u0003|\u0002BREAKING CHANGE: Oops my bad\nrefs: #0001\nrefs: #0002\u0003||\u001e";
        var commit = new Commit("commitSha",
                                ["parent1", "parent2"],
                                summary, "", "",
                                new CommitMessageMetadata("feat", true, "Big red feature", "", footerKeyValues));

        var result = CommitObfuscator.GetObfuscatedLogLine(@"|\  ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }
}