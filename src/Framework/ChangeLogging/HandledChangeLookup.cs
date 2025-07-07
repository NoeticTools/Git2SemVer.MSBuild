using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class HandledChangeLookup
{
    private readonly Dictionary<string, Dictionary<string, HandledChange>> _inner = new();

    public HandledChangeLookup(IEnumerable<HandledChange> handledChanges)
    {
        AddRange(handledChanges);
    }

    public void Add(HandledChange value)
    {
        Add(GetKey(value), value);
    }

    private void AddRange(IEnumerable<HandledChange> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    private bool Contains((string, string) key)
    {
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        return _inner.ContainsKey(key.Item1) && _inner[key.Item1].ContainsKey(key.Item2);
    }

    public bool TryGet(ICommitMessageMetadata messageMetadata, out HandledChange? value)
    {
        return TryGet(GetKey(messageMetadata), out value);
    }

    private HandledChange this[(string, string) key] => _inner[key.Item1][key.Item2];

    private void Add((string, string) key, HandledChange value)
    {
        Dictionary<string, HandledChange> itemsDictionary;
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!_inner.ContainsKey(key.Item1))
        {
            itemsDictionary = new Dictionary<string, HandledChange>();
            _inner.Add(key.Item1, itemsDictionary);
        }
        else
        {
            itemsDictionary = _inner[key.Item1];
        }

        itemsDictionary.Add(key.Item2, value);
    }

    private static (string, string) GetKey(HandledChange value)
    {
        return (value.ChangeType, value.Description);
    }

    private static (string, string) GetKey(ICommitMessageMetadata value)
    {
        return (value.ChangeTypeText, value.ChangeDescription);
    }

    private bool TryGet((string, string) key, out HandledChange? value)
    {
        if (Contains(key))
        {
            value = this[key];
            return true;
        }

        value = null;
        return false;
    }
}