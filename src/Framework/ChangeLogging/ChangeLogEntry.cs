using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeLogEntry : IEquatable<ICommitMessageMetadata>
{
    private readonly HashSet<string> _commitIds = [];
    private readonly List<string> _issues = [];

    public ChangeLogEntry(ICommitMessageMetadata messageMetadata)
    {
        MessageMetadata = messageMetadata;
        AddIssues(messageMetadata.Issues);
    }

    // ReSharper disable once CollectionNeverQueried.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public List<string> CommitIds { get; } = [];

    public IReadOnlyList<string> Issues => _issues;

    public ICommitMessageMetadata MessageMetadata { get; }

    public void AddCommitId(string commitSha)
    {
        CommitIds.Add(commitSha);
    }

    public void AddIssues(IEnumerable<string> issueIds)
    {
        foreach (var issueId in issueIds)
        {
            AddIssue(issueId);
        }
    }

    public bool EqualsChangeTypeAndDescription(ICommitMessageMetadata other)
    {
        return Metadata.ChangeTypeText.Equals(other.ChangeTypeText) &&
               Metadata.ChangeDescription.Equals(other.ChangeDescription);
    }

    public override int GetHashCode()
    {
        return _messageMetadata.GetHashCode();
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
        return other != null && MessageMetadata.Equals(other.MessageMetadata);
    }
}