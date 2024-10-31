using System.Text.RegularExpressions;
using Injectio.Attributes;
using NoeticTools.Common.Exceptions;


#pragma warning disable SYSLIB1045

namespace NoeticTools.Common.Tools.Git;

#pragma warning disable CS1591
public static class ShaObfuscator
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

    public static void Clear()
    {
        ObfuscatedCommitShaLookup.Clear();
    }
}