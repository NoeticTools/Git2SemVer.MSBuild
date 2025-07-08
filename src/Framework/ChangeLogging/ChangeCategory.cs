// ReSharper disable UnusedMember.Global

using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class ChangeCategory(CategorySettings settings, ITextFormatter markdownIssueFormatter)
{
    private readonly List<ChangeLogEntry> _changes = [];
    private readonly Regex _changeTypeRegex = new(settings.ChangeTypePattern);

    public IReadOnlyList<ChangeLogEntry> Changes => _changes;

    public CategorySettings Settings { get; } = settings;

    private void AddRange(IReadOnlyList<ChangeLogEntry> changes)
    {
        _changes.AddRange(changes);
    }

    public void ExtractChangeLogsFrom(List<ICommitMessageMetadata> metatdata)
    {
        var matchingMetadata = metatdata.Where(Matches).ToList();
        matchingMetadata.ForEach(x => metatdata.Remove(x));
        AddRange(GetUniqueChangelogEntries(matchingMetadata));
    }

    private bool Matches(IChangeMessageMetadata messageMetadata)
    {
        return _changeTypeRegex.IsMatch(messageMetadata.ChangeTypeText);
    }

    private IReadOnlyList<ChangeLogEntry> GetUniqueChangelogEntries(List<ICommitMessageMetadata> metadata)
    {
        var changeLogEntries =
            new ChangeLookup<ChangeLogEntry>(logEntry => logEntry.MessageMetadata);
        foreach (var metadataDatum in metadata)
        {
            if (!changeLogEntries.TryGet(metadataDatum, out var logEntry))
            {
                logEntry = new ChangeLogEntry(metadataDatum, markdownIssueFormatter);
                changeLogEntries.Add(logEntry);
            }

            logEntry!.TryAddIssues(metadataDatum.Issues);
        }

        return changeLogEntries.ToList();
    }
}