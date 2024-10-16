using System.Text.RegularExpressions;
using NoeticTools.Common.Exceptions;


#pragma warning disable SYSLIB1045

namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
public static class GitObfuscation
{
    private static readonly Dictionary<string, string> ObfuscatedCommitShaLookup = new();

    public static string GetObfuscatedSha(string sha)
    {
        if (ObfuscatedCommitShaLookup.TryGetValue(sha, out var value))
        {
            return value;
        }

        var newValue = sha.Length > 6 ? (ObfuscatedCommitShaLookup.Count + 1).ToString("D").PadLeft(4, '0') : sha;
        ObfuscatedCommitShaLookup.Add(sha, newValue);
        return newValue;
    }

    /// <summary>
    ///     Get partially obfuscated version of the log line (--pretty="format:|%H|%P|%s|%d|") suitable for logging.
    /// </summary>
    public static string ObfuscateLogLine(string line)
    {
        if (line.Length == 0)
        {
            return line;
        }

        //| (HEAD -> main, tag: v0.3.3, origin/main, origin/HEAD)|
        //(HEAD -> main, tag: v0.3.3, origin/main, origin/HEAD)

        var regex = new Regex(@"^(?<graph>[^\.]*)(\.\|(?<sha>[^\|]*)?\|(?<parents>[^\|]*)?\|(?<summary>[^\|]*)?\|(?<refs>[^\|]*)?\|)?$",
                              RegexOptions.Multiline);
        var match = regex.Match(line.Trim());
        if (!match.Success)
        {
            throw new Git2SemVerGitOperationException($"Unable to obtain obfuscated log line from line: {line}.");
        }

        var graph = GetGroupValue(match, "graph");

        var sha = GetGroupValue(match, "sha");
        var parents = GetGroupValue(match, "parents").Split(' ');
        var refs = GetGroupValue(match, "refs")!;
        var redactedRefs = new Regex(@"HEAD -> \S+?(?=[,\)])").Replace(refs, "HEAD -> REDACTED_BRANCH");
        var redactedRefs2 = new Regex(@"origin\/\S+?(?=[,\)])").Replace(redactedRefs, "origin/REDACTED_BRANCH");
        var parentShas = parents.Length > 0 ? string.Join(" ", parents.Select(GetObfuscatedSha)) : string.Empty;

        return sha.Length == 0 ? $"{graph,-12}" : $"{graph,-15} .|{GetObfuscatedSha(sha)}|{parentShas}|REDACTED|{redactedRefs2}|";
    }

    public static void Reset()
    {
        ObfuscatedCommitShaLookup.Clear();
    }

    private static string GetGroupValue(Match match, string groupName)
    {
        var group = match.Groups[groupName];
        return group.Success ? group.Value : "";
    }
}