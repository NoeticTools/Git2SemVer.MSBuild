using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.ConventionCommits;


// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

/// <summary>
///     Git2SemVer changelog generation settings.
/// </summary>
/// <remarks>
///     Git repository (project) settings. To be located with the git repository.
/// </remarks>
public sealed class ChangelogProjectSettings : IEquatable<ChangelogProjectSettings>
{
    /// <summary>
    ///     Categories to include in the changelog.
    /// </summary>
    [JsonPropertyOrder(20)]
    public CategorySettings[] Categories { get; set; } = [];

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

    public static ChangelogProjectSettings Load(string dataDirectory, string filename)
    {
        var filePath = Path.Combine(dataDirectory, filename);
        if (File.Exists(filePath))
        {
            return Load(filePath);
        }

        var config = new ChangelogProjectSettings
        {
            Categories = ChangelogConstants.DefaultCategories
        };
        config.Save(dataDirectory, filename);
        return config;
    }

    /// <summary>
    ///     Save configuration to file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Saves the user's Git2SemVer configuration file.
    ///     </para>
    /// </remarks>
    public void Save(string dataDirectory, string filename)
    {
        // ReSharper disable once InvertIf
        if (dataDirectory.Length > 0)
        {
            if (Directory.Exists(dataDirectory))
            {
                return;
            }

            Directory.CreateDirectory(dataDirectory);
        }

        var filePath = Path.Combine(dataDirectory, filename);
        Git2SemVerJsonSerializer.Write(filePath, this);
    }

    /// <summary>
    ///     Load the configuration. May return cached configuration.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Loads the user's Git2SemVer configuration file.
    ///         If the file does not exist it is created.
    ///     </para>
    /// </remarks>
    private static ChangelogProjectSettings Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<ChangelogProjectSettings>(filePath);
    }
}