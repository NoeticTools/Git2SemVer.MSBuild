namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

/// <summary>
/// Nested dictionary change log entry lookup.
/// </summary>
public sealed class ChangeLogEntryMessages : NestedDictionary<ChangeLogEntry>
{
    public void Add(ChangeLogEntry value)
    {
        Add((value.Metadata.ChangeTypeText, value.Metadata.ChangeDescription), value);
    }

    public void AddRange(IEnumerable<ChangeLogEntry> logEntries)
    {
        foreach (var logEntry in logEntries)
        {
            Add(logEntry);
        }
    }

    public void Remove(ChangeLogEntry logEntry)
    {
        Remove((logEntry.Metadata.ChangeTypeText, logEntry.Metadata.ChangeDescription));
    }
}