using System.Collections;
using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.ConventionCommits.Json;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

[JsonConverter(typeof(FooterKeyValuesJsonConverter))]
public sealed class FooterKeyValues : IEnumerable<KeyValue>
{
    private readonly List<KeyValue> _items = [];

    public FooterKeyValues()
    {
    }

    public FooterKeyValues(IEnumerable<KeyValue> items)
    {
        _items.AddRange(items);
    }

    public IReadOnlyList<string> this[string key] =>
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        _items.Where(x => string.Equals(key, x.Keyword, StringComparison.InvariantCulture)).Select(x => x.Value).ToReadOnlyList();

    public void Add(string key, string value)
    {
        _items.Add(new KeyValue(key, value));
    }

    public bool ContainsKey(string key)
    {
        return _items.Any(x => string.Equals(key, x.Keyword, StringComparison.InvariantCulture));
    }

    public IEnumerator<KeyValue> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}