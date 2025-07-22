// ReSharper disable ReplaceSubstringWithRangeIndexer

using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.Exceptions;


namespace NoeticTools.Git2SemVer.Core.Tools.Git;

public sealed class CommitId : IEquatable<CommitId>, IEquatable<string>
{
    private const int ShortShaLength = 7;
    private readonly LoadOnDemand<string> _shortSha;

    [JsonConstructor]
    public CommitId()
    {
        _shortSha = new LoadOnDemand<string>(() => Sha.Length < 7 ? Sha : Sha.Substring(0, ShortShaLength));
    }

    public CommitId(string sha) : this()
    {
        if (sha.Length == 0)
        {
            throw new Git2SemVerGitLogParsingException("Empty commit SHA.");
        }

        Sha = sha;
    }

    public string Sha { get; set; } = string.Empty;

    [JsonIgnore]
    public string ShortSha => _shortSha.Value;

    public bool Equals(string? other)
    {
        return !ReferenceEquals(null, other) && Sha.Equals(other);
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

        return Sha == other.Sha;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is CommitId other && Equals(other));
    }

    public override int GetHashCode()
    {
        return Sha.GetHashCode();
    }
}