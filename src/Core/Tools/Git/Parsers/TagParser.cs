﻿using System.Text.RegularExpressions;
using LibGit2Sharp;
using NoeticTools.Git2SemVer.Core.Diagnostics;
using NoeticTools.Git2SemVer.Core.Exceptions;
using Semver;


namespace NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;

/// <summary>
///     Parse tags to identify release tags and return release version.
/// </summary>
#pragma warning disable CS1591
[RegisterSingleton]
public sealed class TagParser : ITagParser
{
    private const string DefaultVersionPrefix = "v";
    private const string VersionPattern = @"(?<version>\d+\.\d+\.\d+)";
    private const string VersionPlaceholder = "%VERSION%";

    public static readonly Dictionary<string, string> ReservedPatternPrefixes = new()
    {
        { "^", "Is not permitted as the format is used with prefix such as `tag: `" },
        { "tag: ", "A prefix found in git log reports" },
        { ".gsm", "A prefix reserved for future Git2SemVer functionality" }
    };

    private readonly Regex _tagVersionFromRefsRegex;
    private readonly Regex _tagVersionRegex;

    public TagParser(string? releaseTagFormat = null)
    {
        var parsePattern = GetParsePattern(releaseTagFormat);
        _tagVersionFromRefsRegex = new Regex($"tag: {parsePattern}", RegexOptions.IgnoreCase);
        _tagVersionRegex = new Regex($"^{parsePattern}", RegexOptions.IgnoreCase);
    }

    public SemVersion? Parse(Tag tag)
    {
        return ParseTagFriendlyName(tag.FriendlyName);
    }

    public IReadOnlyList<SemVersion> Parse(string refs)
    {
        if (refs.Length == 0)
        {
            return [];
        }

        var matches = _tagVersionFromRefsRegex.Matches(refs);
        if (matches.Count == 0)
        {
            return [];
        }

        var versions = new List<SemVersion>();
        foreach (Match match in matches)
        {
            var value = match.Groups["version"].Value;
            var version = SemVersion.Parse(value, SemVersionStyles.Strict);
            versions.Add(version);
        }

        return versions;
    }

    private static string GetParsePattern(string? releaseTagFormat)
    {
        if (string.IsNullOrWhiteSpace(releaseTagFormat))
        {
            return DefaultVersionPrefix + VersionPattern;
        }

        var reservedPrefix =
            ReservedPatternPrefixes.Keys.FirstOrDefault(x => releaseTagFormat!.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
        if (reservedPrefix != null)
        {
            throw new Git2SemVerDiagnosticCodeException(new GSV005(releaseTagFormat!, reservedPrefix));
        }

        if (!releaseTagFormat!.Contains(VersionPlaceholder))
        {
            throw new Git2SemVerDiagnosticCodeException(new GSV006(releaseTagFormat!));
        }

        return releaseTagFormat!.Replace(VersionPlaceholder, VersionPattern);
    }

    internal SemVersion? ParseTagFriendlyName(string friendlyName)
    {
        var match = _tagVersionRegex.Match(friendlyName);
        return !match.Success ? null : SemVersion.Parse(match.Groups["version"].Value, SemVersionStyles.Strict);
    }
}