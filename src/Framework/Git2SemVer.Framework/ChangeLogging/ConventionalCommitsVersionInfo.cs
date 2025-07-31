using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ConventionalCommitsVersionInfo
{
    [JsonConstructor]
    public ConventionalCommitsVersionInfo()
    {
    }

    public ConventionalCommitsVersionInfo(IVersionOutputs outputs, ContributingCommits contributing)
    {
        ContributingReleases = outputs.Git.ContributingReleases.Select(x => x.ToString()).ToArray();
        ConventionalCommits = contributing.Commits.Where(HasConventionalCommitInfo).Select(x => new ConventionalCommit(x)).ToList();
        HeadCommitSha = contributing.Head.CommitId.Sha;
        HeadCommitWhen = contributing.Head.When;
        BranchName = contributing.BranchName;
        Version = outputs.Version!;
        InformationalVersion = outputs.InformationalVersion!;
    }

    [JsonRequired]
    public string BranchName { get; set; } = string.Empty;

    [JsonPropertyOrder(100)]
    [JsonRequired]
    public string[] ContributingReleases { get; set; } = [];

    [JsonPropertyOrder(200)]
    [JsonRequired]
    public IReadOnlyList<ConventionalCommit> ConventionalCommits { get; set; } = [];

    [JsonRequired]
    public string HeadCommitSha { get; set; } = string.Empty;

    [JsonRequired]
    public DateTimeOffset HeadCommitWhen { get; set; } = DateTimeOffset.MinValue;

    [JsonRequired]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion InformationalVersion { get; set; } = new(0, 0, 0);

    [JsonPropertyOrder(-100)]
    [JsonRequired]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion Version { get; set; } = new(0, 0, 0);

    public static ConventionalCommitsVersionInfo Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<ConventionalCommitsVersionInfo>(filePath);
    }

    public void Write(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        Git2SemVerJsonSerializer.Write(filePath, this);
    }

    private static bool HasConventionalCommitInfo(Commit x)
    {
        var changeType = x.MessageMetadata.ChangeType;
        return changeType.Length > 0 &&
               x.MessageMetadata.ApiChangeFlags.Any;
    }
}