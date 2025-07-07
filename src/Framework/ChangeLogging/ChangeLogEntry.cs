using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeLogEntry
{
    private readonly List<string> _issues = [];

    public ChangeLogEntry(ICommitMessageMetadata messageMetadata)
    {
        MessageMetadata = messageMetadata;
        TryAddIssues(messageMetadata.Issues);
    }

    // ReSharper disable once UnusedMember.Global
    public string Description => MessageMetadata.ChangeDescription;

    // ReSharper disable once UnusedMember.Global

    public IReadOnlyList<string> Issues => _issues;

    public ICommitMessageMetadata MessageMetadata { get; }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ChangeLogEntry other && Equals(other));
    }

    private bool Equals(ChangeLogEntry other)
    {
        return GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return MessageMetadata.GetHashCode();
    }

    public void TryAddIssues(IEnumerable<string> issueIds)
    {
        foreach (var issueId in issueIds)
        {
            TryAddIssue(issueId);
        }
    }

    private bool TryAddIssue(string issueId)
    {
        if (_issues.Contains(issueId))
        {
            return false;
        }

        _issues.Add(issueId);
        return true;
    }
}