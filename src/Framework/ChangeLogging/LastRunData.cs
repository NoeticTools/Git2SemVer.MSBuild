using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Framework.Framework;
using Semver;


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

    [JsonPropertyOrder(40)]
    public IReadOnlyList<string> ContributingReleases { get; set; } = [];

    public static string GetFilePath(string dataDirectory, string targetFilePath)
    {
        var targetFilename = targetFilePath.Length == 0 ? "no_target" : Path.GetFileName(targetFilePath);
        return Path.Combine(dataDirectory, targetFilename + ChangelogResources.LastRunFileSuffix);
    }

    public static LastRunData Load(string directory, string filename)
    {
        return Load(GetFilePath(directory, filename));
    }

    public static LastRunData Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<LastRunData>(filePath);
    }

    public void Save(string directory, string filePath)
    {
        Git2SemVerJsonSerializer.Write(GetFilePath(directory, filePath), this);
    }

    public void Save(string filePath)
    {
        Git2SemVerJsonSerializer.Write(filePath, this);
    }

    public void Update(ChangelogInputs outputs)
    {
        HeadSha = outputs.HeadCommitSha;
        CommitWhen = DateTimeOffset.Now;
        SemVersion = outputs.Version!.ToString();
        BranchName = outputs.BranchName;
        ContributingReleases = outputs.ContribReleases.Select(x => x.ToString()).ToReadOnlyList();
    }

    public bool ContributingReleasesChanged(SemVersion[] priorContributingReleases)
    {
        if (ContributingReleases.Count == 0)
        {
            return false; // no data
        }

        if (ContributingReleases.Count != priorContributingReleases.Length)
        {
            return true;
        }

        return !priorContributingReleases.All(ver => ContributingReleases.Contains(ver.ToString()));
    }
}