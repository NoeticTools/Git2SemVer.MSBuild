using System.Text.RegularExpressions;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class TextFormatter(string format) : ITextFormatter
{
    public string Format(string value)
    {
        return string.Format(format, value);
    }
}