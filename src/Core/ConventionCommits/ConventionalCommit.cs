using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public sealed class ConventionalCommit : IChangeTypeAndDescription
{
    [JsonConstructor]
    public ConventionalCommit()
    {
    }

    public ConventionalCommit(Commit commit) : this()
    {
        Description = commit.MessageMetadata.Description;
        ChangeType = commit.MessageMetadata.ChangeType;
        Issues = commit.MessageMetadata.Issues;
        Footer = commit.MessageMetadata.FooterKeyValues;
        Sha = commit.CommitId.Sha;
        BreakingChange = commit.MessageMetadata.ApiChangeFlags.BreakingChange;
    }

    [JsonPropertyOrder(40)]
    public bool BreakingChange { get; set; }

    [JsonPropertyOrder(20)]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyOrder(10)]
    public string ChangeType { get; set; } = string.Empty;

    [JsonPropertyOrder(60)]
    public FooterKeyValues Footer { get; set; } = new();

    [JsonPropertyOrder(50)]
    public IReadOnlyList<string> Issues { get; set; } = [];

    [JsonPropertyOrder(30)]
    public string Sha { get; set; } = string.Empty;
}