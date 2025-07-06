using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeLogEntry
{
    private readonly HashSet<string> _commitIds = [];
    private readonly List<string> _issues = [];

    public ChangeLogEntry(ICommitMessageMetadata messageMetadata)
    {
        Metadata = messageMetadata;
        AddIssues(messageMetadata.FooterKeyValues["issues"]);
        AddIssues(messageMetadata.FooterKeyValues["issue"]);
        AddIssues(messageMetadata.FooterKeyValues["refs"]);
        AddIssues(messageMetadata.FooterKeyValues["ref"]);
    }

    public string Description => Metadata.ChangeDescription;

    public IReadOnlyList<string> Issues => _issues;

    public ICommitMessageMetadata Metadata { get; }

    public void AddCommitId(string commitSha)
    {
        _commitIds.Add(commitSha);
    }

    public void AddIssues(IEnumerable<string> issueIds)
    {
        foreach (var issueId in issueIds)
        {
            AddIssue(issueId);
        }
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ChangeLogEntry other && Equals(other));
    }

    public bool EqualsChangeTypeAndDescription(ICommitMessageMetadata other)
    {
        return Metadata.ChangeTypeText.Equals(other.ChangeTypeText) &&
               Metadata.ChangeDescription.Equals(other.ChangeDescription);
    }

    public override int GetHashCode()
    {
        return Metadata.GetHashCode();
    }

    public bool HasCommitId(string commitSha)
    {
        return _commitIds.Contains(commitSha);
    }

    private void AddIssue(string issueId)
    {
        if (!_issues.Contains(issueId))
        {
            _issues.Add(issueId);
        }
    }

    private bool Equals(ChangeLogEntry? other)
    {
        return other != null && Metadata.Equals(other.Metadata);
    }
}