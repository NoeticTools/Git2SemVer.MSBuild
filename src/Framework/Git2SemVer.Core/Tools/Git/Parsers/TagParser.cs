using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
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
    private const string PriorVersionPattern = @"(?<priorVersion>\d+\.\d+\.\d+)";
    private const string VersionPattern = @"(?<version>\d+\.\d+\.\d+)";
    private const string WaypointTagPrefix = @"(?<waypoint>.git2semver\.waypoint\()";
    private const string WaypointTagSuffix = @"\)\.((?<breaking>break(ing)?)|(?<feat>feat(ure)?)|(?<fix>fix)|none)";

    public static readonly Dictionary<string, string> ReservedPatternPrefixes = new()
    {
        { "^", "Is not permitted as the format is used with prefix such as `tag: `" },
        { "tag: ", "A prefix found in git log reports" },
        { ".git2semver", "A prefix reserved for future Git2SemVer functionality" }
    };

    private readonly Regex _tagVersionRegex;

    public TagParser(string? releaseTagFormat = null)
    {
        var parsePattern = GetParsePattern(releaseTagFormat);
        _tagVersionRegex = new Regex($"^{parsePattern}", RegexOptions.IgnoreCase);
    }

    public TagMetadata ParseTagName(string friendlyName)
    {
        var match = _tagVersionRegex.Match(friendlyName);
        if (!match.Success)
        {
            return new TagMetadata();
        }

        if (match.Groups["waypoint"].Success)
        {
            return CreateWaypointReleaseState(match);
        }

        return new TagMetadata(ReleaseTypeId.Released,
                               SemVersion.Parse(match.Groups["version"].Value, SemVersionStyles.Strict),
                               new ApiChangeFlags());
    }

    private static TagMetadata CreateWaypointReleaseState(Match match)
    {
        var breakingChange = match.Groups["breaking"].Success;
        var featureAdded = match.Groups["feat"].Success;
        var fix = match.Groups["fix"].Success;
        var changes = new ApiChangeFlags(breakingChange, featureAdded, fix);
        var version = SemVersion.Parse(match.Groups["priorVersion"].Value, SemVersionStyles.Strict);
        return new TagMetadata(ReleaseTypeId.ReleaseWaypoint, version, changes);
    }

    private static string GetParsePattern(string? releaseTagFormat)
    {
        if (string.IsNullOrWhiteSpace(releaseTagFormat))
        {
            return $"(({TagParsingConstants.DefaultVersionPrefix}{VersionPattern})|({WaypointTagPrefix}{TagParsingConstants.DefaultVersionPrefix}{PriorVersionPattern}{WaypointTagSuffix}))";
        }

        var versionPattern = ReplacePlaceholderInPattern(releaseTagFormat!);
        var priorVersionPattern = releaseTagFormat!.Replace(TagParsingConstants.VersionPlaceholder, PriorVersionPattern);
        return $"({versionPattern}|({WaypointTagPrefix}{priorVersionPattern}{WaypointTagSuffix}))";
    }

    private static string ReplacePlaceholderInPattern(string releaseTagFormat)
    {
        var reservedPrefix =
            ReservedPatternPrefixes.Keys.FirstOrDefault(x => releaseTagFormat.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
        if (reservedPrefix != null)
        {
            throw new Git2SemVerDiagnosticCodeException(new GSV005(releaseTagFormat!, reservedPrefix));
        }

        if (!releaseTagFormat!.Contains(TagParsingConstants.VersionPlaceholder))
        {
            throw new Git2SemVerDiagnosticCodeException(new GSV006(releaseTagFormat!));
        }

        return releaseTagFormat!.Replace(TagParsingConstants.VersionPlaceholder, VersionPattern);
    }
}