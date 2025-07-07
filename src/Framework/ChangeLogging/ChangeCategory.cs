// ReSharper disable UnusedMember.Global

using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeCategory(CategorySettings settings)
{
    private readonly Regex _changeTypeRegex = new(settings.ChangeTypePattern);
    private readonly List<ChangeLogEntry> _changes = [];

    public IReadOnlyList<ChangeLogEntry> Changes => _changes;

    public CategorySettings Settings { get; } = settings;

    public void AddRange(IReadOnlyList<ChangeLogEntry> changes)
    {
        _changes.AddRange(changes);
    }

    public bool Matches(IChangeMessageMetadata messageMetadata)
    {
        return _changeTypeRegex.IsMatch(messageMetadata.ChangeTypeText);
    }

    public void ExtractFrom(List<Commit> remainingCommits)
    {
        var commits = remainingCommits.Where(x => Matches(x.MessageMetadata)).ToList();
        commits.ForEach(x => remainingCommits.Remove(x));
        AddRange(GetUniqueChangelogEntries(commits));
    }

    private static IReadOnlyList<ChangeLogEntry> GetUniqueChangelogEntries(IReadOnlyList<Commit> commits)
    {
        var changeLogEntries = new ChangeMessageDictionary<ChangeLogEntry>();
        foreach (var commit in commits)
        {
            var messageMetadata = commit.MessageMetadata;
            if (!changeLogEntries.TryGet(messageMetadata, out var logEntry))
            {
                logEntry = new ChangeLogEntry(messageMetadata);
                changeLogEntries.Add(logEntry);
            }
            logEntry!.TryAddIssues(messageMetadata.Issues);
            logEntry.AddCommitId(commit.CommitId.ShortSha);
        }

        return changeLogEntries.ToList();
    }
}