using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Framework.Framework;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Framework.Tests;

[TestFixture]
internal class RegexPatternTests
{
    [Test]
    public void CanObtainGroupNamesTest()
    {
        const string input = "main";
        var regex = new Regex("^((?<release>main|release)|(?<beta>feat(ure)?)|(?<alpha>.*))[\\/_]?");

        var match = regex.Match(input);

        var groupNames = regex.GetGroupNames();
        foreach (var groupName in groupNames)
        {
            if (groupName[0].IsDigit())
            {
                continue;
            }

            TestContext.Out.WriteLine($"  {groupName} - {match.Groups[groupName].Success}");
        }
    }
}