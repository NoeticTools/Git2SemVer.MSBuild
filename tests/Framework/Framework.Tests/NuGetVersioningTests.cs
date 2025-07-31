using NuGet.Versioning;


namespace NoeticTools.Git2SemVer.Framework.Tests;

[TestFixture]
public class NuGetVersioningTests
{
    [TestCase("1.2.3-alpha.001")]
    public void IsInvalidTest(string input)
    {
        var succeeded = NuGetVersion.TryParse(input, out _);

        Assert.That(succeeded, Is.False);
    }

    [TestCase("2.3.5")]
    public void IsNotLegacyNuGetTest(string input)
    {
        var succeeded = NuGetVersion.TryParse(input, out var version);

        Assert.That(succeeded, Is.True);
        Assert.That(version!.IsLegacyVersion, Is.False);
        Assert.That(version.Revision, Is.EqualTo(0));
    }

    [TestCase("1.2.3-alpha1")]
    public void IsNotV2Test(string input)
    {
        var succeeded = NuGetVersion.TryParse(input, out var version);

        Assert.That(succeeded, Is.True);
        Assert.That(version!.IsLegacyVersion, Is.False);
        Assert.That(version.IsSemVer2, Is.False);
        Assert.That(version.IsPrerelease);
    }

    [TestCase("1.2.3-alpha.1")]
    [TestCase("1.2.3-alpha.9010")]
    [TestCase("1.2.3-alpha.Uncontrolled")]
    public void IsNuGetV2Test(string input)
    {
        var succeeded = NuGetVersion.TryParse(input, out var version);

        Assert.That(succeeded, Is.True);
        Assert.That(version!.IsLegacyVersion, Is.False);
        Assert.That(version.IsSemVer2, Is.True);
        Assert.That(version.IsPrerelease);
        Assert.That(version.Revision, Is.EqualTo(0));
    }
}