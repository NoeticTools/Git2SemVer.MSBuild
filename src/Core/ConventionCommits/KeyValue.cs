namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
/// Key value pair for JSON serialisation. Required for .NET Framework support.
/// </summary>
public sealed class KeyValue(string keyword, string value) : IEquatable<KeyValue>
{
    public string Keyword { get; } = keyword;

    public string Value { get; } = value;

    public bool Equals(KeyValue? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Keyword == other.Keyword && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is KeyValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Keyword.GetHashCode() * 397) ^ Value.GetHashCode();
        }
    }

    public static KeyValue? Parse(string text)
    {
        var elements = text.Split(':');
        if (elements.Length != 2)
        {
            return null;
        }

        return new KeyValue(elements[0], elements[1].Trim());
    }

    public override string ToString()
    {
        return $"{Keyword}: {Value}";
    }
}