using NoeticTools.Git2SemVer.Core.Diagnostics;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;
using Semver;


namespace NoeticTools.Git2SemVer.Core.Tests.Tools.Git.Parsers;

[TestFixture]
internal class TagParserTests
{
    private const string VersionPlaceholder = "%VERSION%";

    [TestCase(@"\d+ abc %VERSION%")]
    [TestCase("release: %VERSION%")]
    [TestCase("release tag: %VERSION% abc")]
    [TestCase("%VERSION%")]
    [TestCase("v%VERSION%")]
    public void ConstructorDoesNotThrowExceptionWhenFormatValid(string tagFormat)
    {
        Assert.That(new TagParser(tagFormat), Is.Not.Null);
    }

    [TestCase("x")]
    [TestCase(@"\d+.\d+.\d+")]
    [TestCase("release: ")]
    [TestCase("release tag: ")]
    [TestCase("VERSION")]
    public void ConstructorThrowsExceptionWhenFormatMissingPlaceholder(string tagFormat)
    {
        Assert.Throws(Is.TypeOf<Git2SemVerDiagnosticCodeException>()
                        .And.Property(nameof(Git2SemVerDiagnosticCodeException.DiagCode)).InstanceOf(typeof(GSV006)),
                      () =>
                      {
                          var tagParser = new TagParser(tagFormat);
                      });
    }

    [TestCase("^")]
    [TestCase("^X")]
    [TestCase("^ ")]
    [TestCase("tag: ")]
    [TestCase("TAG: ")]
    [TestCase(".git2semver")]
    [TestCase(".Git2SemVer")]
    [TestCase(".git2semver.something")]
    public void ConstructorThrowsExceptionWhenFormatPrefixInvalid(string tagFormatPrefix)
    {
        Assert.Throws(Is.TypeOf<Git2SemVerDiagnosticCodeException>()
                        .And.Property(nameof(Git2SemVerDiagnosticCodeException.DiagCode)).InstanceOf(typeof(GSV005)),
                      () =>
                      {
                          var tagParser = new TagParser(tagFormatPrefix + VersionPlaceholder);
                      });
    }

    [TestCase(@"\d+ abc %VERSION%", "77 abc 12.34")]
    [TestCase("release: %VERSION%", "release: 123456")]
    [TestCase("release %VERSION%", "release: 12.34.56")]
    [TestCase("release tag: %VERSION% abc", "release tag: 12.34.56 ab")]
    [TestCase("%VERSION%", "v12.34.56")]
    [TestCase("v%VERSION%", "12.34.56")]
    public void ParseTagNameWithInvalidReleaseTag(string tagFormat, string tagText)
    {
        var tagParser = new TagParser(tagFormat);

        var result = tagParser.ParseTagName(tagText);

        Assert.That(result.ReleaseType, Is.EqualTo(ReleaseTypeId.NotReleased));
        Assert.That(result.Version, Is.Null);
    }

    [TestCase("", "v12.34.56")]
    [TestCase(@"\d+ abc %VERSION%", "77 abc 12.34.56")]
    [TestCase("release: %VERSION%", "release: 12.34.56")]
    [TestCase("release tag: %VERSION% abc", "release tag: 12.34.56 abc")]
    [TestCase("%VERSION%", "12.34.56")]
    [TestCase("v%VERSION%", "v12.34.56")]
    public void ParseTagNameWithValidReleaseTag(string tagFormat, string tagText)
    {
        var expected = new SemVersion(12, 34, 56);
        var tagParser = new TagParser(tagFormat);

        var result = tagParser.ParseTagName(tagText);

        Assert.That(result.ReleaseType, Is.EqualTo(ReleaseTypeId.Released));
        Assert.That(result.Version, Is.EqualTo(expected));
    }

    [TestCase("", "v12.34.56")]
    [TestCase(@"\d+ abc %VERSION%", "77 abc 12.34.56")]
    [TestCase("release: %VERSION%", "release: 12.34.56")]
    [TestCase("release tag: %VERSION% abc", "release tag: 12.34.56 abc")]
    [TestCase("%VERSION%", "12.34.56")]
    [TestCase("v%VERSION%", "v12.34.56")]
    public void ParseTagNameWithValidWaypointTag(string tagFormat, string tagText)
    {
        var expected = new SemVersion(12, 34, 56);
        var tagParser = new TagParser(tagFormat);

        var result = tagParser.ParseTagName($".git2semver.waypoint({tagText}).feat");

        Assert.That(result.ReleaseType, Is.EqualTo(ReleaseTypeId.ReleaseWaypoint));
        Assert.That(result.Version, Is.EqualTo(expected));
        Assert.That(result.ChangeFlags.BreakingChange, Is.False);
        Assert.That(result.ChangeFlags.FunctionalityChange, Is.True);
        Assert.That(result.ChangeFlags.Fix, Is.False);
    }

    [TestCase("feat", false, true, false)]
    [TestCase("feature", false, true, false)]
    [TestCase("break", true, false, false)]
    [TestCase("breaking", true, false, false)]
    [TestCase("fix", false, false, true)]
    [TestCase("none", false, false, false)]
    public void ParseTagNameWithValidWaypointTagWithBump(string suffix, bool expectBreak, bool expectFeat, bool expectFix)
    {
        var tagParser = new TagParser();

        var result = tagParser.ParseTagName($".git2semver.waypoint(v1.2.3).{suffix}");

        Assert.That(result.ReleaseType, Is.EqualTo(ReleaseTypeId.ReleaseWaypoint));
        Assert.That(result.Version, Is.EqualTo(new SemVersion(1, 2, 3)));
        Assert.That(result.ChangeFlags.BreakingChange, Is.EqualTo(expectBreak));
        Assert.That(result.ChangeFlags.FunctionalityChange, Is.EqualTo(expectFeat));
        Assert.That(result.ChangeFlags.Fix, Is.EqualTo(expectFix));
    }
}