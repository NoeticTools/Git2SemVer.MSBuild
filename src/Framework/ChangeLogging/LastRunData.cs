using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public sealed class LastRunData
{
    [JsonPropertyOrder(40)]
    // ReSharper disable once MemberCanBePrivate.Global
    public IReadOnlyList<string> ContributingReleases { get; set; } = [];

    [JsonPropertyOrder(50)]
    public List<HandledChange> HandledChanges { get; set; } = [];

    /// <summary>
    ///     This file's schema revision. To allow for file migration.
    /// </summary>
    [JsonPropertyOrder(-10)]
    // ReSharper disable once MemberCanBePrivate.Global
    public string Rev { get; set; } = "";

    [JsonIgnore]
    public bool NoData => string.IsNullOrEmpty(Rev);

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

    public static DirectoryInfo GetFilePath(string dataDirectory, string targetFilePath)
    {
        var targetFilename = targetFilePath.Length == 0 ? "no_target" : Path.GetFileName(targetFilePath);
        return new DirectoryInfo(Path.Combine(dataDirectory, targetFilename + ChangelogConstants.LastRunDataFileSuffix));
    }

    public static LastRunData Load(string directory, string filename)
    {
        return Git2SemVerJsonSerializer.Read<LastRunData>(GetFilePath(directory, filename).FullName);
    }

    public void Save(string directory, string filePath)
    {
        Rev = "1";
        var path = GetFilePath(directory, filePath).FullName;
        Git2SemVerJsonSerializer.Write(path, this);
    }

    public void Update(ConventionalCommitsVersionInfo outputs)
    {
        ContributingReleases = outputs.ContributingReleases.Select(x => x.ToString()).ToReadOnlyList();
    }
}