using NoeticTools.Common.Tools.Git;
using NoeticTools.Testing.Common;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.IntegrationTests;

[TestFixture]
internal class GitToolIntegrationTests
{
    [Test]
    public void RunGetVersionTest()
    {
        var logger = new NUnitTaskLogger();
        var tool = new GitTool(logger);

        var result = tool.Run("--version");

        TestContext.Progress.WriteLine(result.stdOutput);
        Assert.That(result.returnCode, Is.EqualTo(0));
        Assert.That(logger.HasError, Is.False);
    }
}