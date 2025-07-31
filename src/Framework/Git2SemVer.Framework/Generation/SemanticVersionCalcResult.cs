﻿using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Generation;

#pragma warning disable CS1591
public sealed class SemanticVersionCalcResult
{
    private readonly List<SemVersion> _priorVersions = [];

    /// <summary>
    ///     Aggregated change flags from all prior releases to head commit.
    /// </summary>
    public ApiChangeFlags ChangeFlags { get; set; } = new();

    public CommitId PriorReleaseCommitId { get; set; } = new("Null commit");

    public SemVersion PriorReleaseVersion { get; set; } = new(0, 1, 0);

    public IReadOnlyList<SemVersion> PriorVersions => _priorVersions;

    /// <summary>
    ///     The calculated semantic version.
    /// </summary>
    public SemVersion Version { get; set; } = new(0, 0, 0);

    public void AddPriorVersion(SemVersion version)
    {
        _priorVersions.Add(version);
    }

    public override string ToString()
    {
        return $"{PriorReleaseCommitId.ShortSha} -> {Contributing.Head.CommitId.ShortSha}  {PriorReleaseVersion} -> {Version}  bumps: {ChangeFlags}";
    }

    internal ContributingCommits Contributing { get; set; } = ContributingCommits.Null;
}