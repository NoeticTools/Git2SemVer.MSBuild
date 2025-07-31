using System.Text.RegularExpressions;
using NoeticTools.Git2SemVer.Core;
using Semver;


#pragma warning disable SYSLIB1045

namespace NoeticTools.Git2SemVer.Framework.Framework.Semver;

/// <summary>
///     Helper string extensions for working with Semantic Versioning strings.
/// </summary>
public static class SemVersionStringExtensions
{
    /// <summary>
    ///     Convert dot delimited metadata identifiers string to a list of metadata identifiers.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static IReadOnlyList<MetadataIdentifier> ToMetadataIdentifiers(this string metadata)
    {
        return metadata.ToNormalisedSemVerString().Split('.').Select(x => new MetadataIdentifier(x)).ToReadOnlyList();
    }

    /// <summary>
    ///     Replace all Semantic Version non-compliant identifier characters with dash ("-") characters.
    /// </summary>
    public static string ToNormalisedSemVerIdentifier(this string identifier)
    {
        var regex = new Regex("[^a-zA-Z0-9-]");
        return regex.Replace(identifier, "-");
    }

    /// <summary>
    ///     Replace all Semantic Version non-compliant version characters with dash ("-") characters.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static string ToNormalisedSemVerString(this string versionString)
    {
        var regex = new Regex(@"[^a-zA-Z0-9-\.]");
        return regex.Replace(versionString, "-");
    }
}