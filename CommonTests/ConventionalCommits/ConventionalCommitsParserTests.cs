using NoeticTools.Common.ConventionCommits;


#pragma warning disable NUnit2045

namespace NoeticTools.CommonTests.ConventionalCommits;

internal class ConventionalCommitsParserTests
{
    private ConventionalCommitsParser _target;

    [TestCase(
                 """
                 Body - paragraph1

                 Body - paragraph2

                 Body - paragraph2
                 """,
                 """
                 Body - paragraph1

                 Body - paragraph2

                 Body - paragraph2
                 """,
                 "",
                 false)]
    [TestCase(
                 """
                 Body - paragraph1
                 """,
                 "Body - paragraph1",
                 "",
                 false)]
    [TestCase(
                 """
                 Body - paragraph1

                 BREAKING CHANGE: Oops
                 """,
                 "Body - paragraph1",
                 "BREAKING CHANGE|Oops",
                 true)]
    [TestCase(
                 """
                 Body - paragraph1

                 BREAKING CHANGE: Oops very sorry

                 """,
                 "Body - paragraph1",
                 "BREAKING CHANGE|Oops very sorry",
                 true)]
    [TestCase(
                 """
                 Body - paragraph1

                 BREAKING CHANGE: Oops very sorry
                 ref: 1234
                 """,
                 "Body - paragraph1",
                 """
                 BREAKING CHANGE|Oops very sorry
                 ref|1234
                 """,
                 true)]
    [TestCase(
                 """
                 Body - paragraph1
                 """,
                 "Body - paragraph1",
                 "",
                 false)]
    public void BodyMultiLineBodyAndFooterTest(string messageBody,
                                               string expectedBody,
                                               string expectedFooter,
                                               bool hasBreakingChange)
    {
        var result = _target.Parse("feat: Added a real nice feature", messageBody);

        Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.Feature));
        Assert.That(result.ApiChangeFlags.BreakingChange, Is.EqualTo(hasBreakingChange));
        Assert.That(result.ChangeDescription, Is.EqualTo("Added a real nice feature"));
        Assert.That(result.Body, Is.EqualTo(expectedBody));
        var keyValuePairs = GetExpectedKeyValuePairs(expectedFooter);

        Assert.That(result.FooterKeyValues, Is.EquivalentTo(keyValuePairs.ToLookup(k => k.key, v => v.value)));
    }

    private static List<(string key, string value)> GetExpectedKeyValuePairs(string expectedFooter)
    {
        var expectedFooterLines = expectedFooter.Split('\n');
        var keyValuePairs = new List<(string key, string value)>();
        foreach (var line in expectedFooterLines)
        {
            if (line.Length == 0)
            {
                continue;
            }

            var elements = line.Split('|');
            keyValuePairs.Add((key: elements[0], value: elements[1].Trim()));
        }

        return keyValuePairs;
    }

    [TestCase(
                 "BREAKING CHANGE: Oops very sorry",
                 "BREAKING CHANGE|Oops very sorry",
                 true)]
    [TestCase(
                 """
                 BREAKING CHANGE: Oops very sorry
                 refs: 12345
                 """,
                 """
                 BREAKING CHANGE|Oops very sorry
                 refs|12345
                 """,
                 true)]
    public void FooterWithoutBodyTest(string messageBody,
                                               string expectedFooter,
                                               bool hasBreakingChange)
    {
        var result = _target.Parse("feat: Added a real nice feature", messageBody);

        Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.Feature));
        Assert.That(result.ApiChangeFlags.BreakingChange, Is.EqualTo(hasBreakingChange));
        Assert.That(result.ChangeDescription, Is.EqualTo("Added a real nice feature"));
        Assert.That(result.Body, Is.EqualTo(""));
        var keyValuePairs = GetExpectedKeyValuePairs(expectedFooter);

        Assert.That(result.FooterKeyValues, Is.EquivalentTo(keyValuePairs.ToLookup(k => k.key, v => v.value)));
    }

    [TestCase(
                 """
                 Body - paragraph1

                 Body - paragraph2

                 Body - paragraph2
                 """,
                 """
                 Body - paragraph1

                 Body - paragraph2

                 Body - paragraph2
                 """)]
    [TestCase(
                 """
                 Body - paragraph1
                 """,
                 """
                 Body - paragraph1
                 """)]
    public void BodyWithFooterTest(string messageBody,
                                   string expectedBody)
    {
        var result = _target.Parse("feat: Added a real nice feature", messageBody);

        Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.Feature));
        Assert.That(result.ApiChangeFlags.BreakingChange, Is.False);
        Assert.That(result.ChangeDescription, Is.EqualTo("Added a real nice feature"));
        Assert.That(result.Body, Is.EqualTo(expectedBody));
        Assert.That(result.FooterKeyValues, Is.Empty);
    }

    [TestCase("feat:")]
    [TestCase("feat:\n")]
    [TestCase("feat: ")]
    [TestCase("feat: \n")]
    [TestCase("feat:  ")]
    [TestCase("fix:  ")]
    [TestCase("fix!:  ")]
    [TestCase("fix(a scope):  ")]
    public void MalformedSubjectLineConventionalCommitInfoTest(string commitSubject)
    {
        var result = _target.Parse(commitSubject, "");

        Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.Unknown));
    }

    [SetUp]
    public void SetUp()
    {
        _target = new ConventionalCommitsParser();
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("feat")]
    [TestCase("feat This is a commit without conventional commit info")]
    [TestCase("This is a commit without conventional commit info")]
    public void SubjectLineWithoutConventionalCommitInfoTest(string commitSubject)
    {
        var result = _target.Parse(commitSubject, "");

        Assert.That(result.ChangeType, Is.EqualTo(CommitChangeTypeId.Unknown));
    }

    [TestCase("feat: Added a real nice feature", CommitChangeTypeId.Feature, "Added a real nice feature", false)]
    [TestCase("feat!: Added a real nice feature", CommitChangeTypeId.Feature, "Added a real nice feature", true)]
    [TestCase("fix: Fixed nasty bug", CommitChangeTypeId.Fix, "Fixed nasty bug", false)]
    [TestCase("build: Build work", CommitChangeTypeId.Build, "Build work", false)]
    [TestCase("chore: Did something", CommitChangeTypeId.Chore, "Did something", false)]
    [TestCase("ci: Did something", CommitChangeTypeId.ContinuousIntegration, "Did something", false)]
    [TestCase("docs: Did something", CommitChangeTypeId.Documentation, "Did something", false)]
    [TestCase("style: Did something", CommitChangeTypeId.Style, "Did something", false)]
    [TestCase("refactor: Did something", CommitChangeTypeId.Refactoring, "Did something", false)]
    [TestCase("perf: Did something ", CommitChangeTypeId.Performance, "Did something ", false)]
    [TestCase("test: Did something", CommitChangeTypeId.Testing, "Did something", false)]
    public void SubjectWithConventionalCommitInfoTest(string messageSubject,
                                                      CommitChangeTypeId expectedChangeTypeId,
                                                      string expectedChangeDescription,
                                                      bool hasBreakingChange)
    {
        var result = _target.Parse(messageSubject, "");

        Assert.That(result.ChangeType, Is.EqualTo(expectedChangeTypeId));
        Assert.That(result.ApiChangeFlags.BreakingChange, Is.EqualTo(hasBreakingChange));
        Assert.That(result.ChangeDescription, Is.EqualTo(expectedChangeDescription));
        Assert.That(result.Body, Is.Empty);
        Assert.That(result.FooterKeyValues, Is.Empty);
    }
}