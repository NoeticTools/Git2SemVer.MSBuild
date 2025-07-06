using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class LastRunData
{
    [JsonIgnore]
    private static readonly Mutex FileMutex = new(false, "G2SemVerChangelogConfigFileMutex");

    [JsonIgnore]
    private static readonly JsonSerializerOptions SerialiseOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    public string Rev { get; set; } = "1";

    public string Head { get; set; } = "";

    public DateTimeOffset CommitWhen { get; set; } = DateTimeOffset.MinValue;

    public string SemVersion { get; set; } = "";

    public string BranchName { get; set; } = "";

    public List<HandledChange> HandledChanges { get; set; } = [];

    public static LastRunData Load(string filePath)
    {
        FileMutex.WaitOne(TimeSpan.FromSeconds(10));
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<LastRunData>(json)!;
            }
            else
            {
                return new LastRunData();
            }
        }
        finally
        {
            FileMutex.ReleaseMutex();
        }
    }

    public void Update(VersionOutputs outputs, ContributingCommits contributing)
    {
        Head = contributing.Head.CommitId.ShortSha;
        CommitWhen = DateTimeOffset.Now;
        SemVersion = outputs.Version!.ToString();
        BranchName = contributing.BranchName;
    }

    public static string GetFilePath(string dataDirectory, string targetFilePath)
    {
        var targetFilename = targetFilePath.Length == 0 ? "console" : Path.GetFileName(targetFilePath);
        return Path.Combine(dataDirectory, targetFilename + ".g2sv.run.json");
    }

    public void Save(string filePath)
    {
        // todo - remove duplication with ChangelogSettings
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