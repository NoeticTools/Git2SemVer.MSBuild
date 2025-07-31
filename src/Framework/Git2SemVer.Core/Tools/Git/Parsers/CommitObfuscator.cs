#pragma warning disable SYSLIB1045

namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

#pragma warning disable CS1591
public sealed class CommitObfuscator : ICommitObfuscator
{
    private readonly Dictionary<string, string> _obfuscatedShaLookup = new();

    public string GetObfuscatedSha(string sha)
    {
        if (_obfuscatedShaLookup.TryGetValue(sha, out var value))
        {
            return value;
        }

        var newValue = sha.Length > 6 ? (_obfuscatedShaLookup.Count + 1).ToString("D").PadLeft(4, '0') : sha;
        _obfuscatedShaLookup.Add(sha, newValue);
        return newValue;
    }
}