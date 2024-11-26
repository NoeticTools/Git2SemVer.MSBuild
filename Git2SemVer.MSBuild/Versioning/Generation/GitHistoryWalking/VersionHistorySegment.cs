using System.Dynamic;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using Semver;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

internal sealed class VersionHistorySegment
{
    private readonly List<Commit> _commits = [];
    private readonly ILogger _logger;
    private ApiChanges? _bumps;

    internal VersionHistorySegment(int id, List<Commit> commits, ILogger logger) : this(id, logger)
    {
        _commits.AddRange(commits);
    }

    internal VersionHistorySegment(int id, ILogger logger)
    {
        _logger = logger;
        Id = id;
    }

    public ApiChanges ApiChangeFlags => GetApiChanges();

    public IReadOnlyList<Commit> Commits => _commits.ToList();

    /// <summary>
    ///     First (oldest) commit in the segment.
    /// </summary>
    public Commit OldestCommit => _commits.Last();

    /// <summary>
    ///     Parent (older) commits that link to this segment.
    ///     If more than one, a merge commit.
    /// </summary>
    public IReadOnlyList<CommitId> ParentCommits => OldestCommit.Parents.ToList(); // todo - >>> remove this ... want to list commits

    /// <summary>
    ///     An arbitrary but unique segment ID.
    /// </summary>
    public int Id { get; }

    /// <summary>
    ///     Last (youngest) commit in the segment.
    /// </summary>
    public Commit YoungestCommit => _commits[0];

    public SemVersion? TaggedReleasedVersion => _commits.Count != 0 ? OldestCommit.ReleasedVersion : null;

    /// <summary>
    ///     Append prior (younger) commit to the segment.
    /// </summary>
    public void Append(Commit youngerCommit)
    {
        if (_commits.Count > 0 && OldestCommit.Parents.All(x => x.Id != youngerCommit.CommitId.Id))
        {
            throw new
                InvalidOperationException($"Cannot append {youngerCommit.CommitId.Id} as it is not connected to segment's first (oldest) commit.");
        }

        _bumps = null;
        _commits.Add(youngerCommit);
        _logger.LogTrace("Commit {0} added to segment {1}.", youngerCommit.CommitId.Id, Id);
    }

    /// <summary>
    ///     A branch has been found from the given commit to the given segment.
    /// </summary>
    public VersionHistorySegment? BranchesFrom(VersionHistorySegment branchSegment, Commit commit, IVersionHistorySegmentFactory segmentFactory)
    {
        _logger.LogDebug("Commit {0} in segment {1} branches to segment {2}:", commit.CommitId.Id, Id, branchSegment.Id);
        using (_logger.EnterLogScope())
        {
            if (commit.CommitId.Equals(YoungestCommit.CommitId))
            {
                _logger.LogTrace("Commit {0} is last (youngest) commit in segment {1}. Link segments.", commit.CommitId.Id, Id);
                return null;
            }

            _bumps = null;
            var fromSegment = SplitSegmentAt(commit, segmentFactory);
            return fromSegment;
        }
    }

    public override string ToString()
    {
        var commitsCount = $"({_commits.Count})";

        var release = TaggedReleasedVersion != null ? TaggedReleasedVersion.ToString() : 
            (ParentCommits.Any() ? "" : "0.1.0");

        return
            $"Segment {Id,-3} {YoungestCommit.CommitId.Id} -> {OldestCommit.CommitId.Id}  {commitsCount,5}   {ApiChangeFlags}    {release}";
    }

    private ApiChanges GetApiChanges()
    {
        if (_bumps != null)
        {
            return _bumps;
        }

        var bumps = new ApiChanges();
        foreach (var commit in _commits)
        {
            if (!commit.HasReleaseTag)
            {
                bumps.Aggregate(commit.Metadata.ApiChangeFlags);
            }
        }

        _bumps = bumps;
        return bumps;
    }

    private VersionHistorySegment SplitSegmentAt(Commit commit, IVersionHistorySegmentFactory segmentFactory)
    {
        var index = _commits.IndexOf(commit);
        if (index < 0)
        {
            throw new Git2SemVerInvalidOperationException("Cannot split a segment that does not contain the commit.");
        }

        using (_logger.EnterLogScope())
        {
            var keepCommits = _commits.Take(index).ToList();
            var olderSegmentCommits = _commits.Skip(index).Take(_commits.Count - index).ToList();
            _commits.Clear();
            _commits.AddRange(keepCommits);

            var olderSegment = segmentFactory.Create(olderSegmentCommits);
            _logger.LogTrace("Split out new segment {2} from segment {0} at commit {1}.",
                             Id, commit.CommitId.Id,
                             olderSegment.Id);

            return olderSegment;
        }
    }
}