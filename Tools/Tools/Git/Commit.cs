using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using NoeticTools.Common.ConventionCommits;
using Semver;


#pragma warning disable SYSLIB1045

// ReSharper disable MergeIntoPattern

namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
public class Commit : ICommit
{
    private const string TagVersionPrefix = "v";
    private readonly Regex _tagVersionRegex = new(@$"tag: {TagVersionPrefix}(?<version>\d+\.\d+\.\d+)", RegexOptions.IgnoreCase);

    public Commit(string sha, string[] parents, string summary, string messageBody, string refs, CommitMessageMetadata metadata)
    {
        CommitId = new CommitId(sha);

        if (parents.Length == 1 && parents[0].Length == 0)
        {
            Parents = [];
        }
        else
        {
            Parents = parents.Select(x => new CommitId(x)).ToArray();
        }

        Refs = refs;

        Summary = summary;
        MessageBody = messageBody;
        Metadata = metadata;
        ReleasedVersion = GetReleaseTag();
    }

    [JsonPropertyOrder(11)]
    public CommitId CommitId { get; }

    [JsonIgnore]
    public bool HasReleaseTag => ReleasedVersion != null;

    [JsonPropertyOrder(22)]
    public string MessageBody { get; }

    [JsonPropertyOrder(90)]
    public CommitMessageMetadata Metadata { get; }

    [JsonIgnore]
    public static Commit Null => new("00000000", [], "null commit", "", "", new CommitMessageMetadata());

    [JsonPropertyOrder(31)]
    public CommitId[] Parents { get; }

    [JsonPropertyOrder(25)]
    public string Refs { get; }

    [JsonPropertyOrder(12)]
    public SemVersion? ReleasedVersion { get; }

    [JsonPropertyOrder(21)]
    public string Summary { get; }

    private SemVersion? GetReleaseTag()
    {
        if (Refs.Length == 0)
        {
            return null;
        }

        var matches = _tagVersionRegex.Matches(Refs);
        if (matches.Count == 0)
        {
            return null;
        }

        var versions = new List<SemVersion>();
        foreach (Match match in matches)
        {
            var version = SemVersion.Parse(match.Groups["version"].Value, SemVersionStyles.Strict);
            versions.Add(version);
        }

        return versions.OrderByDescending(x => x, new SemverSortOrderComparer()).FirstOrDefault();
    }
}

internal sealed class SemverSortOrderComparer : IComparer<SemVersion>
{
    public int Compare(SemVersion x, SemVersion y)
    {
        return x.CompareSortOrderTo(y);
    }
}