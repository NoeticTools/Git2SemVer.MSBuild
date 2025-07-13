using NoeticTools.Git2SemVer.Tool.CommandLine;


namespace NoeticTools.Git2SemVer.Tool.Tests.CommandLine;

[TestFixture]
[NonParallelizable]
internal class Git2SemVerCommandAppTests
{
    [Test]
    public void Test()
    {
        Git2SemVerCommandApp.Execute(["--version"]);
    }
}