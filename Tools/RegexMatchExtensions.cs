using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace NoeticTools.Common
{
    public static class RegexMatchExtensions
    {
        public static string GetGroupValue(this Match match, string groupName)
        {
            var group = match.Groups[groupName];
            return group.Success ? group.Value : "";
        }
    }
}
