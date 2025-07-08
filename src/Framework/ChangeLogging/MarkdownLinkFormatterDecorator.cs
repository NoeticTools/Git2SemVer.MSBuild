namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class MarkdownLinkFormatterDecorator(ITextFormatter inner) : ITextFormatter
{
    public string Format(string value)
    {
        if (value.Contains('/'))
        {
            return value;
        }

        var link = inner.Format(value);
        return string.Equals(link, value, StringComparison.InvariantCulture) ? link : $"[{value}]({link})";
    }
}