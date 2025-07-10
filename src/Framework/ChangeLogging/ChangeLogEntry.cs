using NoeticTools.Git2SemVer.Core.ConventionCommits;
// ReSharper disable UnusedMember.Global


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeLogEntry
{
    private readonly ITextFormatter _markdownIssueFormatter;
    private readonly List<string> _issues = [];

    public ChangeLogEntry(ConventionalCommit messageMetadata, ITextFormatter markdownIssueFormatter)
    {
        _markdownIssueFormatter = markdownIssueFormatter;
        MessageMetadata = messageMetadata;
        TryAddIssues(messageMetadata.Issues);
    }

    public string Description => MessageMetadata.ChangeDescription;

    // ReSharper disable once MemberCanBePrivate.Global
    public IReadOnlyList<string> Issues => _issues;

    /// <summary>
    /// Issues as Markdown links when issue url is provided.
    /// </summary>
    public IReadOnlyList<string> IssuesMarkdown => Issues.Select(x => _markdownIssueFormatter.Format(x)).ToList();

    public ConventionalCommit MessageMetadata { get; }

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