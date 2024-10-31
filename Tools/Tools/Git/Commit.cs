using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Semver;
#pragma warning disable SYSLIB1045


// ReSharper disable MergeIntoPattern

namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
public class Commit : ICommit
{
    private const string TagVersionPrefix = "v";

    private readonly string _refs;
    private readonly Regex _tagVersionRegex = new(@$"tag: {TagVersionPrefix}(?<version>\d+\.\d+\.\d+)", RegexOptions.IgnoreCase);

    public Commit(string sha, string[] parents, string summary, string messageBody, string refs)
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

        _refs = refs;

        Summary = summary;
        MessageBody = messageBody;
        ReleasedVersion = GetReleaseTag();

        //ChangeType = // conventional commit
        //ChangeDescription = // conventional commit
        //HasBreakingChange = // conventional commit
    }

    public CommitId CommitId { get; }

    public string Summary { get; }

    public string MessageBody { get; }

    [JsonIgnore]
    public static Commit Null => new("00000000", [], "null commit", "", "");

    public CommitId[] Parents { get; }

    public SemVersion? ReleasedVersion { get; }

    private SemVersion? GetReleaseTag()
    {
        if (_refs.Length == 0)
        {
            return null;
        }

        var matches = _tagVersionRegex.Matches(_refs);
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