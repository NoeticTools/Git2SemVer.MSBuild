using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using NoeticTools.Git2SemVer.MSBuild.Tools.CI;


namespace NoeticTools.Git2SemVer.MSBuild.Framework.Config;

/// <summary>
///     User's local Git2SemVer configuration.
/// </summary>
internal sealed class Git2SemVerConfiguration : IConfiguration
{
    [JsonIgnore]
    private static int _instanceHash;

    [JsonIgnore]
    private static Git2SemVerConfiguration? _instance;

    [JsonPropertyOrder(100)]
    public List<Git2SemVerBuildLogEntry> BuildLog { get; set; } = [];

    /// <summary>
    ///     The local build log size.
    ///     If zero, the local build log is cleared and disabled.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Default is 0 (disabled).
    ///     </para>
    /// </remarks>
    [JsonPropertyOrder(95)]
    public int BuildLogSizeLimit { get; set; }

    /// <summary>
    ///     The next local build number. Default is 1.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <b>Not recommended for use on build system (controlled) build hosts.</b>
    ///     </para>
    ///     <para>
    ///         Used by <a cref="UncontrolledHost">UncontrolledHost.BuildNumber</a>/
    ///     </para>
    /// </remarks>
    [JsonPropertyOrder(10)]
    public int BuildNumber { get; set; } = 1;

    /// <summary>
    ///     This configuration's schema version.
    /// </summary>
    [JsonPropertyOrder(1)]
    public int Rev { get; set; } = 1;

    public Git2SemVerBuildLogEntry AddLogEntry(string buildNumber, bool hasLocalChanges, string branch, string lastCommitId, string path)
    {
        if (BuildLog.Count >= BuildLogSizeLimit)
        {
            BuildLog.RemoveRange(BuildLogSizeLimit, BuildLog.Count - BuildLogSizeLimit);
        }

        var entry = new Git2SemVerBuildLogEntry(buildNumber, hasLocalChanges, branch, lastCommitId, path);
        if (BuildLogSizeLimit > 0)
        {
            BuildLog.Add(entry);
        }

        return entry;
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
    public static Git2SemVerConfiguration Load()
    {
        if (_instance != null)
        {
            return _instance;
        }

        var filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _instance = Load(json);
        }
        else
        {
            _instance = new Git2SemVerConfiguration();
        }

        _instanceHash = _instance!.GetCurrentHashCode();
        return _instance!;
    }

    internal static Git2SemVerConfiguration Load(string json)
    {
        return JsonSerializer.Deserialize<Git2SemVerConfiguration>(json)!;
    }

    /// <summary>
    ///     Save configuration to file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Saves the user's Git2SemVer configuration file.
    ///     </para>
    /// </remarks>
    public void Save()
    {
        if (_instanceHash == _instance!.GetCurrentHashCode())
        {
            return;
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };

        var json = JsonSerializer.Serialize(this, options);
        json = Regex.Unescape(json);
        File.WriteAllText(GetFilePath(), json);
    }

    private int GetCurrentHashCode()
    {
        return HashCode.Combine(BuildLog, BuildLogSizeLimit, BuildNumber, Rev);
    }

    private static string GetFilePath()
    {
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Git2SemVer");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, "Configuration.json");
    }
}