namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

/// <summary>
/// Key value pair for JSON serialisation. Required for .NET Framework support.
/// </summary>
public sealed class KeyValue : IEquatable<KeyValue>
{
    public string Keyword { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

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
}