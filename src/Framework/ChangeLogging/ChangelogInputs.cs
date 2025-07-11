using System.Reflection;
using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;
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
        ContribReleases = outputs.Git.ContributingReleases.Select(x => x.ToString()).ToArray();
        ConventionalCommits = contributing.Commits.Where(HasConventionalCommitInfo).Select(x => new ConventionalCommit(x)).ToList();
        HeadCommitSha = contributing.Head.CommitId.Sha;
        HeadCommitWhen = contributing.Head.When;
        BranchName = contributing.BranchName;
        Version = outputs.Version!;
        InformationalVersion = outputs.InformationalVersion!;
    }

    private static bool HasConventionalCommitInfo(Commit x)
    {
        var changeType = x.MessageMetadata.ChangeType;
        return changeType.Length > 0 &&
               x.MessageMetadata.ApiChangeFlags.Any;
    }

    [JsonRequired]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? InformationalVersion { get; set; } = null;

    [JsonRequired]
    public string BranchName { get; set; } = string.Empty;

    [JsonPropertyOrder(200)]
    [JsonRequired]
    public IReadOnlyList<ConventionalCommit> ConventionalCommits { get; set; } = [];

    [JsonPropertyOrder(100)]
    [JsonRequired]
    public string[] ContribReleases { get; set; } = [];

    [JsonRequired]
    public string HeadCommitSha { get; set; } = string.Empty;

    [JsonRequired]
    public DateTimeOffset HeadCommitWhen { get; set; } = DateTimeOffset.MinValue;

    [JsonPropertyOrder(-100)]
    [JsonRequired]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? Version { get; set; } = null;

    public void Save(string filePath)
    {
        Git2SemVerJsonSerializer.Write(filePath, this);
    }

    public static ChangelogInputs Load(string filePath)
    {
        return Git2SemVerJsonSerializer.Read<ChangelogInputs>(filePath);
    }
}