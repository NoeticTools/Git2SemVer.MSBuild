using System.Text.RegularExpressions;


namespace NoeticTools.Common;

public static class RegexMatchExtensions
{
    public static string GetGroupValue(this Match match, string groupName)
    {
        if (!match.Success)
        {
            return "";
        }

        var group = match.Groups[groupName];
        return group.Success ? group.Value : "";
    }
}