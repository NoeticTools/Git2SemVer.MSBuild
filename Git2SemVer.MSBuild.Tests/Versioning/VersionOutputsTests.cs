using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning;

[TestFixture]
internal class VersionOutputsTests
{
    [SetUp]
    public void SetUp()
    {
        CommitObfuscator.Clear();
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

        var result = GeneratedVersionsJsonFile.ToJson(target);

        Assert.That(result, Is.Not.Null);

        const string expected = """
                                {
                                  "Rev": 1,
                                  "Git2SemVerVersionInfo": {
                                    "AssemblyVersion": "10.11.12",
                                    "BuildContext": "CONTEXT",
                                    "BuildNumber": "777",
                                    "BuildSystemVersion": "5.6.7-TEST",
                                    "FileVersion": null,
                                    "Git": {
                                      "$type": "GitOutputs",
                                      "BranchName": "",
                                      "CommitsSinceLastRelease": 0,
                                      "HasLocalChanges": false,
                                      "HeadCommit": {
                                        "$type": "Commit",
                                        "CommitId": {
                                          "Id": "00000000",
                                          "ObfuscatedSha": "0001",
                                          "ShortSha": "0000000"
                                        },
                                        "ReleasedVersion": null,
                                        "Summary": "null commit",
                                        "MessageBody": "",
                                        "Refs": "",
                                        "Parents": [],
                                        "Metadata": {
                                          "ApiChangeFlags": {
                                            "BreakingChange": false,
                                            "Fix": false,
                                            "FunctionalityChange": false
                                          },
                                          "Body": "",
                                          "ChangeDescription": "",
                                          "ChangeType": 0,
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