using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Persistence;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Tests.Generation;

[TestFixture]
internal class VersionOutputsTests
{
    [SetUp]
    public void SetUp()
    {
        //CommitObfuscator.Clear();
    }

    [Test]
    public void CanDeserialiseRev2()
    {
        const string content = """
                               {
                                 "Rev": 2,
                                 "Git2SemVerVersionInfo": {
                                   "AssemblyVersion": "10.11.12",
                                   "BuildContext": "CONTEXT",
                                   "BuildNumber": "777",
                                   "BuildSystemVersion": "5.6.7-TEST",
                                   "FileVersion": null,
                                   "Git": {
                                     "$type": "GitOutputs",
                                     "BranchName": "",
                                     "HasLocalChanges": false,
                                     "HeadCommit": {
                                       "$type": "Commit",
                                       "CommitId": {
                                         "Sha": "00000000",
                                         "ShortSha": "0000000"
                                       },
                                       "ReleasedVersion": null,
                                       "Summary": "null commit",
                                       "MessageBody": "",
                                       "Parents": [],
                                       "Metadata": {
                                         "ApiChangeFlags": {
                                           "BreakingChange": false,
                                           "Fix": false,
                                           "FunctionalityChange": false
                                         },
                                         "Body": "",
                                         "ChangeDescription": "",
                                         "ChangeType": 1,
                                         "FooterKeyValues": []
                                       }
                                     },
                                     "LastReleaseCommit": null,
                                     "LastReleaseVersion": null
                                   },
                                   "InformationalVersion": null,
                                   "IsInInitialDevelopment": false,
                                   "Output1": "",
                                   "Output2": "",
                                   "PackageVersion": null,
                                   "PrereleaseLabel": "",
                                   "Version": null
                                 }
                               }
                               """;

        var result = OutputsJsonFileIO.FromJson(content);

        Assert.That(result.AssemblyVersion!.ToString(), Is.EqualTo("10.11.12"));
        Assert.That(result.BuildContext, Is.EqualTo("CONTEXT"));
        Assert.That(result.BuildNumber, Is.EqualTo("777"));
    }

    [Test]
    public void CanSerialise()
    {
        var target = new VersionOutputs
        {
            AssemblyVersion = new Version(10, 11, 12),
            BuildContext = "CONTEXT",
            BuildNumber = "777",
            BuildSystemVersion = new SemVersion(5, 6, 7).WithPrerelease("TEST")
        };

        var result = OutputsJsonFileIO.ToJson(target);

        Assert.That(result, Is.Not.Null);

        const string expected = """
                                {
                                  "Rev": 3,
                                  "Git2SemVerVersionInfo": {
                                    "Version": "0.0.0",
                                    "InformationalVersion": "0.0.0",
                                    "PackageVersion": "0.0.0",
                                    "BuildSystemVersion": "5.6.7-TEST",
                                    "AssemblyVersion": "10.11.12",
                                    "FileVersion": "0.0.0",
                                    "BuildNumber": "777",
                                    "PrereleaseLabel": "",
                                    "BuildContext": "CONTEXT",
                                    "IsInInitialDevelopment": false,
                                    "Git": {
                                      "$type": "GitOutputs",
                                      "BranchName": "",
                                      "HasLocalChanges": false,
                                      "HeadCommit": {
                                        "$type": "Commit",
                                        "CommitId": {
                                          "Sha": "00000000"
                                        },
                                        "TagMetadata": {
                                          "ReleaseType": 1,
                                          "Version": null,
                                          "ChangeFlags": {
                                            "BreakingChange": false,
                                            "FunctionalityChange": false,
                                            "Fix": false
                                          }
                                        },
                                        "Summary": "null commit",
                                        "Parents": [],
                                        "MessageMetadata": {
                                          "ApiChangeFlags": {
                                            "BreakingChange": false,
                                            "FunctionalityChange": false,
                                            "Fix": false
                                          },
                                          "Body": "",
                                          "Description": "",
                                          "ChangeType": "",
                                          "Scope": "",
                                          "FooterKeyValues": []
                                        }
                                      },
                                      "LastReleaseCommit": null,
                                      "LastReleaseVersion": null,
                                      "ContributingReleases": []
                                    },
                                    "Output1": "",
                                    "Output2": ""
                                  }
                                }
                                """;
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void SetAllVersionPropertiesFrom()
    {
        var target = new VersionOutputs();
        var informationalVersion = new SemVersion(0, 5, 6).WithPrerelease("Beta-InitialDev", "77")
                                                          .WithMetadata("METADATA");

        target.SetAllVersionPropertiesFrom(informationalVersion, "BUILD_NUMBER", "BUILD_CONTEXT");

        Assert.That(target.InformationalVersion, Is.EqualTo(informationalVersion));
        Assert.That(target.PackageVersion, Is.EqualTo(informationalVersion.WithoutMetadata()));
    }
}