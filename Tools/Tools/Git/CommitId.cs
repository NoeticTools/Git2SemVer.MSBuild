// ReSharper disable ReplaceSubstringWithRangeIndexer

using NoeticTools.Common.Exceptions;


namespace NoeticTools.Common.Tools.Git;

public sealed class CommitId : IEquatable<CommitId>, IEquatable<string>
{
    private const int ShortShaLength = 7;

    internal CommitId(string sha)
    {
        if (sha.Length == 0)
        {
            throw new Git2SemVerGitLogParsingException("Empty commit SHA.");
        }
        Id = sha;
        ShortSha = ToShortSha(sha);
        ObfuscatedSha = ShaObfuscator.GetObfuscatedSha(Id);
    }

    public string Id { get; }

    public string ObfuscatedSha { get; }

    public string ShortSha { get; }

    public bool Equals(string? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return Id.Equals(other);
    }

    public bool Equals(CommitId? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is CommitId other && Equals(other));
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static string ToShortSha(string sha)
    {
        return sha.Length < 7 ? sha : sha.Substring(0, ShortShaLength);
    }
}