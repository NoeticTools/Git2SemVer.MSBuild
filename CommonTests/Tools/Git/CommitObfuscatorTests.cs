using NoeticTools.Common.ConventionCommits;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.CommonTests.Tools.Git;

internal class CommitObfuscatorTests
{
    [Test]
    public void NoConventionalCommitInfoLogLineTest()
    {
        const string expected = "*               \u001f.|0001|0002 0003|\u0002REDACTED\u0003|\u0002\u0003||";
        var commit = new Commit("commitSha", ["parent1", "parent2"], "Summary line", "", "", new CommitMessageMetadata());

        var result = CommitObfuscator.GetObfuscatedLogLine("* ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }

    [SetUp]
    public void SetUp()
    {
        CommitObfuscator.Clear();
    }

    [Test]
    public void FirstShaIs0001Test()
    {
        var result = CommitObfuscator.GetObfuscatedSha("1234567");

        Assert.That(result, Is.EqualTo("0001"));
    }

    [TestCase("0099")]
    [TestCase("123456")]
    public void ShortShaIsReturnedSameTest(string sha)
    {
        var result = CommitObfuscator.GetObfuscatedSha(sha);

        Assert.That(result, Is.EqualTo(sha));
    }

    [Test]
    public void ReturnsPriorObfuscatedShaTest()
    {
        const string sha1 = "fa340bd213c0001";
        const string sha2 = "fa340bd213c0002";
        const string sha3 = "fa340bd213c0003";

        var result11 = CommitObfuscator.GetObfuscatedSha(sha1);
        var result12 = CommitObfuscator.GetObfuscatedSha(sha1);
        var result21 = CommitObfuscator.GetObfuscatedSha(sha2);
        var result31 = CommitObfuscator.GetObfuscatedSha(sha3);
        var result22 = CommitObfuscator.GetObfuscatedSha(sha2);

        Assert.That(result11, Is.EqualTo(result12));
        Assert.That(result21, Is.EqualTo(result22));
        Assert.That(result11, Is.Not.EqualTo(result21));
        Assert.That(result21, Is.Not.EqualTo(result31));
    }

    [Test]
    public void WithConventionalCommitSummaryLogLineTest()
    {
        const string summary = "feat!: Big red feature";
        var expected = $"|\\              \u001f.|0001|0002 0003|\u0002{summary}\u0003|\u0002\u0003||";
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
        var footerKeyValues = new List<(string key, string value)>
        {
            ("BREAKING CHANGE", "Oops my bad"),
            ("refs", "#0001"),
            ("refs", "#0002")
        };
        var expected =
            $"|\\              \u001f.|0001|0002 0003|\u0002{summary}\u0003|\u0002BREAKING CHANGE: Oops my bad\nrefs: #0001\nrefs: #0002\u0003||";
        var commit = new Commit("commitSha",
                                ["parent1", "parent2"],
                                summary, "", "",
                                new CommitMessageMetadata("feat", true, "Big red feature", "", footerKeyValues));

        var result = CommitObfuscator.GetObfuscatedLogLine(@"|\  ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void WithRefsLogLineTest()
    {
        const string summary = "fix: Fixed";
        var footerKeyValues = new List<(string key, string value)>();
        var expected =
            $"|\\              \u001f.|0001|0002 0003|\u0002{summary}\u0003|\u0002\u0003| (HEAD -> REDACTED_BRANCH, origin/main)|";
        var commit = new Commit("commitSha",
                                ["parent1", "parent2"],
                                summary, "", "HEAD -> REDACTED_BRANCH, origin/main",
                                new CommitMessageMetadata("feat", true, "Big red feature", "", footerKeyValues));

        var result = CommitObfuscator.GetObfuscatedLogLine(@"|\  ", commit);

        Assert.That(result, Is.EqualTo(expected));
    }
}