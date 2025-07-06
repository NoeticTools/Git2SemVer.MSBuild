using System.Text.Json;
using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.ConventionCommits;


// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

/// <summary>
///     Settings to configure how changelog is generated.
/// </summary>
public sealed class ChangelogLocalSettings : IEquatable<ChangelogLocalSettings>
{
    [JsonPropertyOrder(10)]
    public ConventionalCommitsSettings ConvCommits { get; set; } = new ConventionalCommitsSettings();

    /// <summary>
    ///     Categories to include in the changelog.
    /// </summary>
    [JsonPropertyOrder(20)]
    public ChangelogCategorySettings[] Categories { get; set; } = [];

    /// <summary>
    ///     Configuration file schema revision.
    /// </summary>
    [JsonPropertyOrder(-10)]
    public string Rev { get; set; } = "1";

    public bool Equals(ChangelogLocalSettings? other)
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
        return ReferenceEquals(this, obj) || (obj is ChangelogLocalSettings other && Equals(other));
    }

    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return HashCode.Combine(Rev, Categories);
        // ReSharper restore NonReadonlyMemberInGetHashCode
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
    public static ChangelogLocalSettings Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<ChangelogLocalSettings>(filePath);
    }

    /// <summary>
    ///     Save configuration to file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Saves the user's Git2SemVer configuration file.
    ///     </para>
    /// </remarks>
    public void Save(string filePath)
    {
        Git2SemVerJsonSerializer.Write(filePath, this);
    }

}