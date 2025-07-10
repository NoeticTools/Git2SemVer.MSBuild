using System.Reflection;
using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;

public class ChangelogInputs
{
    [JsonConstructor]
    public ChangelogInputs()
    {
    }

    public ChangelogInputs(VersionOutputs outputs, ContributingCommits contributing)
    {
        ContribReleases = outputs.Git.ContributingReleases;
        Commits = contributing.Commits.Where(x => x.MessageMetadata.ChangeType.Length > 0).Select(x => new ConventionalCommit(x)).ToList();
        HeadCommitSha = contributing.Head.CommitId.Sha;
        HeadCommitWhen = contributing.Head.When;
        BranchName = contributing.BranchName;
        Version = outputs.Version!;
        InformationalVersion = outputs.InformationalVersion!;
    }

    [JsonRequired]
    public SemVersion InformationalVersion { get; set; } = null!;

    [JsonRequired]
    public string BranchName { get; set; } = string.Empty;

    [JsonPropertyOrder(200)]
    [JsonRequired]
    public IReadOnlyList<ConventionalCommit> Commits { get; set; } = [];

    [JsonPropertyOrder(100)]
    [JsonRequired]
    public SemVersion[] ContribReleases { get; set; } = [];

    [JsonRequired]
    public string HeadCommitSha { get; set; } = string.Empty;

    [JsonRequired]
    public DateTimeOffset HeadCommitWhen { get; set; } = DateTimeOffset.MinValue;

    [JsonPropertyOrder(-100)]
    [JsonRequired]
    public SemVersion Version { get; set; } = null!;

    public void Save(string filePath)
    {
        Git2SemVerJsonSerializer.Write(filePath, this);
    }
}