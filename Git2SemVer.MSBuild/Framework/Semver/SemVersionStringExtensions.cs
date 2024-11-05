using System.Text.RegularExpressions;
using Semver;


#pragma warning disable SYSLIB1045

namespace NoeticTools.Git2SemVer.MSBuild.Framework.Semver;

/// <summary>
///     Helper string extensions for working with Semantic Versioning strings.
/// </summary>
public static class SemVersionStringExtensions
{
    /// <summary>
    ///     Returns true if the identifier complies to Semantic Versioning 2.0 allowed characters.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Semantic Versioning 2.0 states: "Identifiers MUST comprise only ASCII alphanumerics and hyphens [0-9A-Za-z-].
    ///         Identifiers MUST NOT be empty. "
    ///     </para>
    /// </remarks>
    public static bool IsAlphanumericOrHyphens(this string identifier)
    {
        var regex = new Regex(@"^[a-zA-Z0-9-]*$");
        return regex.IsMatch(identifier);
    }

    /// <summary>
    ///     Is this string composed entirely of ASCII digits '0' to '9'?
    /// </summary>
    public static bool IsDigits(this string value)
    {
        foreach (var c in value)
            if (!c.IsDigit())
            {
                return false;
            }

        return true;
    }

    /// <summary>
    ///     Convert dot delimited metadata identifiers string to a list of metadata identifiers.
    /// </summary>
    public static IReadOnlyList<MetadataIdentifier> ToMetadataIdentifiers(this string metadata)
    {
        return metadata.ToNormalisedSemVerString().Split('.').Select(x => new MetadataIdentifier(x)).ToReadOnlyList();
    }

    /// <summary>
    ///     Create dot delimited metadata identifiers string from a list of metadata identifiers.
    /// </summary>
    public static string ToMetadataString(this IReadOnlyList<MetadataIdentifier> metadata)
    {
        return string.Join(".", metadata);
    }

    /// <summary>
    ///     Replace all Semantic Version non-compliant identifier characters with dash ("-") characters.
    /// </summary>
    public static string ToNormalisedSemVerIdentifier(this string identifier)
    {
        var regex = new Regex(@"[^a-zA-Z0-9-]");
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

    /// <summary>
    ///     Convert dot delimited prerelease identifiers string to a list of prerelease identifiers.
    /// </summary>
    public static IReadOnlyList<PrereleaseIdentifier> ToPrereleaseIdentifiers(this string prerelease)
    {
        return prerelease.ToNormalisedSemVerString().Split('.').Select(x => new PrereleaseIdentifier(x)).ToReadOnlyList();
    }

    /// <summary>
    ///     Create dot delimited prerelease identifiers string from a list of prerelease identifiers.
    /// </summary>
    public static string ToPrereleaseString(this IReadOnlyList<PrereleaseIdentifier> prerelease)
    {
        return string.Join(".", prerelease);
    }
}