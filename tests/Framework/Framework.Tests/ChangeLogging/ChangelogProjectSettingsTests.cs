using NoeticTools.Git2SemVer.Framework.ChangeLogging;


namespace NoeticTools.Git2SemVer.Framework.Tests.ChangeLogging;

[TestFixture]
public class ChangelogProjectSettingsTests
{
    [Test]
    public void JsonRoundTripTest()
    {
        var original = new ChangelogProjectSettings
        {
            DataDirectory = "data_directory",
            OutputFilePath = "output/path",
            ArtifactLinkPattern = "http://some_url.domain/%VERSION%",
            ConvCommits =
            {
                FooterIssueTokens = ["token1", "token2"]
            },
            Categories =
            [
                new CategorySettings(1, "Section", "tagName")
            ]
        };
        var json = original.ToJson();

        var copy = ChangelogProjectSettings.FromJson(json);

        Assert.That(copy, Is.EqualTo(original));
        Assert.That(copy.ConvCommits.GetHashCode(), Is.EqualTo(original.ConvCommits.GetHashCode()));
        Assert.That(copy.GetHashCode(), Is.EqualTo(original.GetHashCode()));
        Assert.That(copy, Is.Not.SameAs(original));
    }

    [Test]
    public void ToJsonWithDefaultsTest()
    {
        var settings = new ChangelogProjectSettings();

        var json = settings.ToJson();

        Console.WriteLine(json);

        Assert.That(json, Is.EqualTo("""
                                     {
                                       "Rev": "1",
                                       "OutputFilePath": "CHANGELOG.md",
                                       "DataDirectory": ".git2semver",
                                       "ArtifactLinkPattern": "",
                                       "IssueLinkFormat": "{0}",
                                       "ConvCommits": {
                                         "FooterIssueTokens": [
                                           "issues",
                                           "issue",
                                           "ref",
                                           "refs"
                                         ]
                                       },
                                       "Categories": [
                                         {
                                           "ChangeTypePattern": "feat",
                                           "Name": "Added",
                                           "Order": 1
                                         },
                                         {
                                           "ChangeTypePattern": "change",
                                           "Name": "Changed",
                                           "Order": 2
                                         },
                                         {
                                           "ChangeTypePattern": "deprecate",
                                           "Name": "Depreciated",
                                           "Order": 3
                                         },
                                         {
                                           "ChangeTypePattern": "remove",
                                           "Name": "Removed",
                                           "Order": 4
                                         },
                                         {
                                           "ChangeTypePattern": "fix",
                                           "Name": "Fixed",
                                           "Order": 5
                                         },
                                         {
                                           "ChangeTypePattern": "security",
                                           "Name": "Security",
                                           "Order": 6
                                         },
                                         {
                                           "ChangeTypePattern": "^(?!dev|Dev|refactor).*$",
                                           "Name": "Other",
                                           "Order": 7
                                         }
                                       ]
                                     }
                                     """));
    }
}