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

    public bool BreakingChange { get; set; }

    public string Description { get; set; } = string.Empty;

    public string ChangeType { get; set; } = string.Empty;

    public FooterKeyValues Footer { get; set; } = new();

    public IReadOnlyList<string> Issues { get; set; } = [];

    public string Sha { get; set; } = string.Empty;
}