using System.Collections;
using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public sealed class FooterKeyValues
{
    // ReSharper disable once MemberCanBePrivate.Global
    public List<KeyValue> Items { get; set; }= [];

    public IReadOnlyList<string> this[string key]
    {
        get
        {
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
            return Items.Where(x => string.Equals(key, x.Keyword, StringComparison.InvariantCulture)).Select(x => x.Value).ToReadOnlyList();
        }
    }

    public bool ContainsKey(string key)
    {
        return Items.Any(x => string.Equals(key, x.Keyword, StringComparison.InvariantCulture));
    }

    public void Add(string key, string value)
    {
        Items.Add(new KeyValue() {Keyword = key, Value = value});
    }
}