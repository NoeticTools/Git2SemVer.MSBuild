using NoeticTools.Git2SemVer.Framework.Framework.Config;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Framework.Tests.Config;

[TestFixture]
internal class Git2SemVerConfigurationTests
{
    [Test]
    public void V0CanLoadTest()
    {
        var json = """
                   {
                     "Version": "1.0.0",
                     "BuildNumber": 1639
                   }
                   """;

        var result = Git2SemVerConfiguration.Load(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Rev, Is.EqualTo(1));
        Assert.That(result.BuildNumber, Is.EqualTo(1639));
        Assert.That(result.BuildLogSizeLimit, Is.EqualTo(0));
    }

    [Test]
    public void V1CanLoadTest()
    {
        var json = """
                   {
                     "Rev": 1,
                     "BuildNumber": 1639
                   }
                   """;

        var result = Git2SemVerConfiguration.Load(json);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Rev, Is.EqualTo(1));
        Assert.That(result.BuildNumber, Is.EqualTo(1639));
        Assert.That(result.BuildLogSizeLimit, Is.EqualTo(0));
    }
}