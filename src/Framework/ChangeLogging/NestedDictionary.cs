namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class NestedDictionary<TV> : INestedDictionary<(string, string), TV>
{
    private readonly Dictionary<string, Dictionary<string, TV>> _inner = new();

    public int Count => _inner.Select(x => x.Value).Sum(x => x.Count);

    public bool TryGet((string, string) key, out TV? value)
    {
        if (Contains(key))
        {
            value = this[key];
            return true;
        }

        value = default(TV);
        return false;
    }

    public IReadOnlyList<TV> GetAll()
    {
        return _inner.SelectMany(x => x.Value.Select(y => y.Value)).ToList();
    }

    public void Add((string, string) key, TV value)
    {
        Dictionary<string, TV> entriesOfType;
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!_inner.ContainsKey(key.Item1))
        {
            entriesOfType = new Dictionary<string, TV>();
            _inner.Add(key.Item1, entriesOfType);
        }
        else
        {
            entriesOfType = _inner[key.Item1];
        }

        entriesOfType.Add(key.Item2, value);
    }

    public bool Contains((string, string) key)
    {
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        return _inner.ContainsKey(key.Item1) && _inner[key.Item1].ContainsKey(key.Item2);
    }

    public TV this[(string, string) key]
    {
        get
        {
            return _inner[key.Item1][key.Item2];
        }
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
}