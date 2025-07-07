using NoeticTools.Git2SemVer.Core.ConventionCommits;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

internal sealed class ChangeLookup<T>
{
    private readonly Dictionary<string, Dictionary<string, T>> _inner = new();
    private readonly Func<T, IChangeTypeAndDescription> _keyLookup;

    public ChangeLookup(Func<T, IChangeTypeAndDescription> keyLookup)
        : this([], keyLookup)
    {
    }

    public ChangeLookup(IEnumerable<T> handledChanges, Func<T, IChangeTypeAndDescription> keyLookup)
    {
        _keyLookup = keyLookup;
        AddRange(handledChanges);
    }

    public void Add(T value)
    {
        Add(GetKey(_keyLookup(value)), value);
    }

    public IReadOnlyList<T> ToList()
    {
        return _inner.SelectMany(x => x.Value.Select(y => y.Value)).ToList();
    }

    public bool TryGet(ICommitMessageMetadata messageMetadata, out T? value)
    {
        return TryGet(GetKey(messageMetadata), out value);
    }

    private T this[(string, string) key] => _inner[key.Item1][key.Item2];

    private void Add((string, string) key, T value)
    {
        Dictionary<string, T> itemsDictionary;
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!_inner.ContainsKey(key.Item1))
        {
            itemsDictionary = new Dictionary<string, T>();
            _inner.Add(key.Item1, itemsDictionary);
        }
        else
        {
            itemsDictionary = _inner[key.Item1];
        }

        itemsDictionary.Add(key.Item2, value);
    }

    private void AddRange(IEnumerable<T> items)
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

    private static (string, string) GetKey(IChangeTypeAndDescription value)
    {
        return (value.ChangeTypeText, value.ChangeDescription);
    }

    private static (string, string) GetKey(ICommitMessageMetadata value)
    {
        return (value.ChangeTypeText, value.ChangeDescription);
    }

    private bool TryGet((string, string) key, out T? value)
    {
        if (Contains(key))
        {
            value = this[key];
            return true;
        }

        value = default;
        return false;
    }
}