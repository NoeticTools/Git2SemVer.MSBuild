using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;


// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

/// <summary>
///     Settings to configure how changelog is generated.
/// </summary>
public sealed class ChangelogSettings : IEquatable<ChangelogSettings>
{
    [JsonIgnore]
    private static readonly Mutex FileMutex = new(false, "G2SemVerChangelogConfigFileMutex");

    [JsonIgnore]
    private static readonly JsonSerializerOptions SerialiseOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    /// <summary>
    ///     Categories to include in the changelog.
    /// </summary>
    public ChangelogCategorySettings[] Categories { get; set; } = [];

    /// <summary>
    ///     Information on last changelog generate run.
    /// </summary>
    public LastRunData LastRun { get; set; } = new();

    /// <summary>
    ///     Configuration file schema revision.
    /// </summary>
    public string Rev { get; set; } = "1";

    public bool Equals(ChangelogSettings? other)
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
        return ReferenceEquals(this, obj) || (obj is ChangelogSettings other && Equals(other));
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
    public static ChangelogSettings Load(string filePath)
    {
        ChangelogSettings instance;

        FileMutex.WaitOne(TimeSpan.FromSeconds(10));
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                instance = JsonSerializer.Deserialize<ChangelogSettings>(json)!;
            }
            else
            {
                instance = new ChangelogSettings();
            }
        }
        finally
        {
            FileMutex.ReleaseMutex();
        }

        return instance;
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
        var json = JsonSerializer.Serialize(this, SerialiseOptions);
        json = Regex.Unescape(json);

        FileMutex.WaitOne(TimeSpan.FromSeconds(10));
        try
        {
            File.WriteAllText(filePath, json);
        }
        finally
        {
            FileMutex.ReleaseMutex();
        }
    }
}