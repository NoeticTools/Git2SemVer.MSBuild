using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.ConventionCommits;


// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

/// <summary>
///     Git2SemVer changelog generation settings.
/// </summary>
/// <remarks>
///     Git repository (project) settings. To be located with the git repository.
/// </remarks>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ChangelogProjectSettings : JsonSettingsFileBase<ChangelogProjectSettings>, IEquatable<ChangelogProjectSettings>
{
    /// <summary>
    ///     Categories to include in the changelog.
    /// </summary>
    [JsonPropertyOrder(20)]
    public CategorySettings[] Categories { get; set; } =
    [
        new(1, "Added", "feat"),
        new(2, "Changed", "change"),
        new(3, "Depreciated", "deprecate"),
        new(4, "Removed", "remove"),
        new(5, "Fixed", "fix"),
        new(6, "Security", "security"),
        new(7, "Other", "^(?!dev|Dev|refactor).*$")
    ];

    [JsonPropertyOrder(10)]
    public ConventionalCommitsSettings ConvCommits { get; set; } = new();

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Issue link format with issue ID as argument ({0}).
    /// </summary>
    /// <remarks>
    ///     Example:
    ///     <example>
    ///         "https://organisation-name/project-name/issues/{0}"
    ///     </example>
    /// </remarks>
    [JsonPropertyOrder(5)]
    public string IssueLinkFormat { get; set; } = "{0}";

    /// <summary>
    ///     Configuration file schema revision.
    /// </summary>
    [JsonPropertyOrder(-10)]
    // ReSharper disable once MemberCanBePrivate.Global
    public string Rev { get; set; } = "1";

    public bool Equals(ChangelogProjectSettings? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rev == other.Rev && Categories.Equals(other.Categories);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ChangelogProjectSettings other && Equals(other));
    }

    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return HashCode.Combine(Rev, Categories);
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}