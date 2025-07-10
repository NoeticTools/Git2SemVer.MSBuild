using System.Text.Json.Serialization;
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
        Commits = contributing.Commits;
        HeadCommitSha = contributing.Head.CommitId.Sha;
        HeadCommitWhen = contributing.Head.When;
        BranchName = contributing.BranchName;
        Version = outputs.Version!;
    }

    [JsonRequired]
    public string BranchName { get; set; } = string.Empty;

    [JsonRequired]
    public IReadOnlyList<Commit> Commits { get; set; } = [];

    [JsonRequired]
    public SemVersion[] ContribReleases { get; set; } = [];

    [JsonRequired]
    public string HeadCommitSha { get; set; } = string.Empty;

    [JsonRequired]
    public DateTimeOffset HeadCommitWhen { get; set; } = DateTimeOffset.MinValue;

    [JsonRequired]
    public SemVersion Version { get; set; } = null!;
}