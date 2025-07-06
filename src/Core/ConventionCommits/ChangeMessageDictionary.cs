namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public class ChangeMessageDictionary<T> where T : IObjectWithMessageMetadata
{
    private readonly Dictionary<string, Dictionary<string, T>> _inner = new();

    public int Count => _inner.Select(x => x.Value).Sum(x => x.Count);

    public T this[(string, string) key] => _inner[key.Item1][key.Item2];

    public void Add(T value)
    {
        Add(GetKey(value), value);
    }

    public void Add((string, string) key, T value)
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

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public bool Contains((string, string) key)
    {
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        return _inner.ContainsKey(key.Item1) && _inner[key.Item1].ContainsKey(key.Item2);
    }

    public IReadOnlyList<T> GetAll()
    {
        return _inner.SelectMany(x => x.Value.Select(y => y.Value)).ToList();
    }

    public void Remove(T item)
    {
        Remove(GetKey(item));
    }

    public void Remove((string, string) key)
    {
        var valueDictionary = _inner[key.Item1];
        valueDictionary.Remove(key.Item2);
        if (valueDictionary.Count == 0)
        {
            _inner.Remove(key.Item1);
        }
    }

    public void AddRangeUnique(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            TryAdd(item);
        }
    }

    private bool TryAdd(T item)
    {
        var key = GetKey(item);
        if (Contains(key))
        {
            return false;
        }
        Add(key, item);
        return true;
    }

    public bool TryGet((string, string) key, out T? value)
    {
        if (Contains(key))
        {
            value = this[key];
            return true;
        }

        value = default;
        return false;
    }

    private static (string, string) GetKey(T value)
    {
        return GetKey(value.MessageMetadata);
    }

    private static (string, string) GetKey(ICommitMessageMetadata value)
    {
        return (value.ChangeTypeText, value.ChangeDescription);
    }

    public bool TryGet(ICommitMessageMetadata messageMetadata, out T? value)
    {
        return TryGet(GetKey(messageMetadata), out value);
    }
}