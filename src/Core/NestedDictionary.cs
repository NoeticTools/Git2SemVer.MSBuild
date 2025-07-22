// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global

namespace NoeticTools.Git2SemVer.Core;

/// <summary>
///     A dictionary of dictionaries both using string keys. The value key is of type (string, string).
/// </summary>
public class NestedDictionary<T>
{
    private readonly Dictionary<string, Dictionary<string, T>> _inner = new();

    public int Count => _inner.Select(x => x.Value).Sum(x => x.Count);

    public T this[(string, string) key] => _inner[key.Item1][key.Item2];

    public void Add((string, string) key, T value)
    {
        Dictionary<string, T> entriesOfType;
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!_inner.ContainsKey(key.Item1))
        {
            entriesOfType = new Dictionary<string, T>();
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

    /// <summary>
    ///     Get all values.
    /// </summary>
    public IReadOnlyList<T> GetAll()
    {
        return _inner.SelectMany(x => x.Value.Select(y => y.Value)).ToList();
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
}