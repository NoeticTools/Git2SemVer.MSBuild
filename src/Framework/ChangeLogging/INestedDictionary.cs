namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public interface INestedDictionary<TK, TV>
{
    bool TryGet(TK key, out TV? value);

    IReadOnlyList<TV> GetAll();

    void Add(TK key, TV value);

    bool Contains((string changeType, string changeDescription) key);

    int Count { get; }

    TV this[(string changeType, string changeDescription) key] { get; }

    void Remove((string, string) key);
}