using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Framework.Generation;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class LastRunData
{
    [JsonPropertyOrder(40)]
    public string BranchName { get; set; } = "";

    [JsonPropertyOrder(20)]
    public DateTimeOffset CommitWhen { get; set; } = DateTimeOffset.MinValue;

    [JsonPropertyOrder(50)]
    public List<HandledChange> HandledChanges { get; set; } = [];

    [JsonPropertyOrder(10)]
    public string HeadSha { get; set; } = "";

    [JsonPropertyOrder(-10)]
    public string Rev { get; set; } = "1.0.0";

    [JsonPropertyOrder(30)]
    public string SemVersion { get; set; } = "";

    public static string GetFilePath(string dataDirectory, string targetFilePath)
    {
        var targetFilename = targetFilePath.Length == 0 ? "no_target" : Path.GetFileName(targetFilePath);
        return Path.Combine(dataDirectory, targetFilename + ".g2sv.data.json");
    }

    public static LastRunData Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<LastRunData>(filePath);
    }

    public void Save(string filePath)
    {
        Git2SemVerJsonSerializer.Write(filePath, this);
    }

    public void Update(VersionOutputs outputs)
    {
        HeadSha = outputs.Git.HeadCommit.CommitId.Sha;
        CommitWhen = DateTimeOffset.Now;
        SemVersion = outputs.Version!.ToString();
        BranchName = outputs.Git.BranchName;
    }
}