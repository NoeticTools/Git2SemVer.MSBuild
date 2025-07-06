using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeLogEntry : IObjectWithMessageMetadata
{
    private readonly List<string> _issues = [];

    public ChangeLogEntry(ICommitMessageMetadata messageMetadata)
    {
        MessageMetadata = messageMetadata;
        AddIssues(messageMetadata.Issues);
    }

    // ReSharper disable once CollectionNeverQueried.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public List<string> CommitIds { get; } = [];

    // ReSharper disable once UnusedMember.Global
    public string Description => MessageMetadata.ChangeDescription;

    // ReSharper disable once UnusedMember.Global
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

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ChangeLogEntry other && Equals(other));
    }

    public override int GetHashCode()
    {
        return MessageMetadata.GetHashCode();
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