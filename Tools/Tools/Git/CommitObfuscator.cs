using System.Text.RegularExpressions;
using NoeticTools.Common.ConventionCommits;


#pragma warning disable SYSLIB1045

namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
public static class CommitObfuscator
{
    private static readonly Dictionary<string, string> ObfuscatedCommitShaLookup = new();

    public static void Clear()
    {
        ObfuscatedCommitShaLookup.Clear();
    }

    public static string GetObfuscatedLogLine(string graph, Commit? commit)
    {
        if (commit == null)
        {
            return $"{graph,-12}";
        }

        var redactedRefs = new Regex(@"HEAD -> \S+?(?=[,\)])").Replace(commit.Refs, "HEAD -> REDACTED_BRANCH");
        var redactedRefs2 = new Regex(@"origin\/\S+?(?=[,\)])").Replace(redactedRefs, "origin/REDACTED_BRANCH");
        if (redactedRefs2.Length > 0)
        {
            redactedRefs2 = $" ({redactedRefs2})";
        }

        var parentShas = commit.Parents.Length > 0 ? string.Join(" ", commit.Parents.Select(x => x.ObfuscatedSha)) : string.Empty;
        var sha = commit.CommitId.ObfuscatedSha;
        var summary = commit.Metadata.ChangeType == CommitChangeTypeId.Unknown ? "REDACTED" : commit.Summary;
        var footer = string.Join("\n", commit.Metadata.FooterKeyValues.SelectMany((kv, _) => kv.Select(value => kv.Key + ": " + value)));
        return $"{graph,-15} \u001f.|{sha}|{parentShas}|\u0002{summary}\u0003|\u0002{footer}\u0003|{redactedRefs2}|";
    }

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
}