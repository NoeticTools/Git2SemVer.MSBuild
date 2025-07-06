using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class CommitMessages : NestedDictionary<Commit>
{
    public bool TryAdd(Commit value)
    {
        var key = (value.MessageMetadata.ChangeTypeText, value.MessageMetadata.ChangeDescription);
        if (Contains(key))
        {
            return false;
        }
        Add(key, value);
        return true;
    }

    public void TryAddRange(IEnumerable<Commit> logEntries)
    {
        foreach (var logEntry in logEntries)
        {
            TryAdd(logEntry);
        }
    }

    public void Remove(Commit commit)
    {
        Remove((commit.MessageMetadata.ChangeTypeText, commit.MessageMetadata.ChangeDescription));
    }
}