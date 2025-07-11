using NoeticTools.Git2SemVer.Core.ConventionCommits;
using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class HandledChange : IChangeTypeAndDescription
{
    [JsonPropertyOrder(20)]
    public string Description { get; set; } = "";

    [JsonPropertyOrder(10)]
    public string ChangeType { get; set; } = "";

    [JsonPropertyOrder(30)]
    public List<string> Issues { get; set; } = [];

    public bool TryAddIssues(IEnumerable<string> issues)
    {
        var newIssues = issues.Where(x => !Issues.Contains(x)).ToList();
        Issues.AddRange(newIssues);
        return newIssues.Count > 0;
    }
}