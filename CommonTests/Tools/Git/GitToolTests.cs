using NoeticTools.Common.Tools.Git;


namespace NoeticTools.CommonTests.Tools.Git;

[TestFixture]
internal class GitToolTests
{
    [Test]
    public void ParseStatusResponseTest()
    {
        const string response = """
                                ## My-Branch/Thing_A
                                 M Git2SemVer.IntegrationTests/Resources/Scripts/ForceProperties1.csx
                                 D Git2SemVer.MSBuild/Framework/ReadOnlyList.cs
                                 D Git2SemVer.MSBuild/Versioning/Generation/Builders/IVersioningContext.cs
                                ?? CommonTests/Tools/Git/
                                ?? Git2SemVer.MSBuild/Versioning/Generation/ApiChanges.cs
                                ?? Git2SemVer.MSBuild/Versioning/Generation/VersioningModeEnum.cs
                                """;

        var result = GitTool.ParseStatusResponseBranchName(response);

        Assert.That(result, Is.EqualTo("My-Branch/Thing_A"));
    }
}