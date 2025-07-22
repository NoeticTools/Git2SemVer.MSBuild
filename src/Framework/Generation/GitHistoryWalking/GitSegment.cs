using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using Semver;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;

/// <summary>
///     A git segment is a collection of continuous commits between commit merges and commits that are branched or are
///     released.
/// </summary>
/// <remarks>
///     <p>
///         The segments newest commit is one of:
///     </p>
///     <list type="bullet">
///         <item>
///             A branched commit.
///         </item>
///         <item>
///             Repository head or target commit.
///         </item>
///     </list>
///     <p>
///         The segments oldest commit is one of:
///     </p>
///     <list type="bullet">
///         <item>
///             A merge commit.
///         </item>
///         <item>
///             Release commit.
///         </item>
///         <item>
///             Repository root commit.
///         </item>
///     </list>
/// </remarks>
internal sealed class GitSegment
{
    private readonly List<Commit> _commits = [];
    private readonly ILogger _logger;
    private ApiChangeFlags? _bumps;

    internal GitSegment(int id, Commit[] commits, ILogger logger)
    {
        _commits.AddRange(commits);
        _logger = logger;
        Id = id;
    }

    /// <summary>
    ///     Aggregation API change flags in this segment.
    /// </summary>
    public ApiChangeFlags ApiChangeFlags => GetApiChanges();

    /// <summary>
    ///     Commits in this segment.
    /// </summary>
    public IReadOnlyList<Commit> Commits => _commits;

    /// <summary>
    ///     An arbitrary but unique segment ID.
    /// </summary>
    public int Id { get; }

    /// <summary>
    ///     Indicates if this commit commit is a release or root commit.
    /// </summary>
    public bool IsAReleaseSegment => Version != null ||
                                     (_commits.Count != 0 && OldestCommit.TagMetadata.IsRootCommit);

    /// <summary>
    ///     First (oldest) commit in the segment.
    /// </summary>
    public Commit OldestCommit => _commits.Last();

    /// <summary>
    ///     Parent (older) commits that link to this segment.
    ///     If more than one, a merge commit.
    /// </summary>
    public IReadOnlyList<CommitId> ParentCommits => OldestCommit.Parents.ToList();

    /// <summary>
    ///     If the oldest commit has a release tag or a waypoint tag this is the version read from that tag,
    ///     otherwise null.
    /// </summary>
    public SemVersion? Version => _commits.Count != 0 ? OldestCommit.TagMetadata.Version : null;

    /// <summary>
    ///     Last (youngest) commit in the segment.
    /// </summary>
    public Commit YoungestCommit => _commits[0];

    /// <summary>
    ///     Append prior (younger) commit to the segment.
    /// </summary>
    public void Append(Commit youngerCommit)
    {
        if (_commits.Count > 0 && OldestCommit.Parents.All(x => x.Sha != youngerCommit.CommitId.Sha))
        {
            throw new
                Git2SemVerInvalidOperationException($"Cannot append {youngerCommit.CommitId.ShortSha} as it is not connected to segment's first (oldest) commit.");
        }

        _bumps = null;
        _commits.Add(youngerCommit);
    }

    /// <summary>
    ///     A branch has been found from the given commit to the given segment.
    /// </summary>
    public GitSegment? BranchesFrom(GitSegment branchSegment, Commit commit, IGitSegmentFactory segmentFactory)
    {
        _logger.LogTrace("Commit {0} in segment {1} branches to segment {2}:", commit.CommitId.ShortSha, Id, branchSegment.Id);
        using (_logger.EnterLogScope())
        {
            if (commit.CommitId.Equals(YoungestCommit.CommitId))
            {
                _logger.LogTrace("Commit {0} is last (youngest) commit in segment {1}.", commit.CommitId.ShortSha, Id);
                return null;
            }

            _bumps = null;
            var fromSegment = SplitAt(commit, segmentFactory);
            return fromSegment;
        }
    }

    public override string ToString()
    {
        var commitsCount = $"{_commits.Count}";

        var release = Version != null ? Version.ToString() :
            ParentCommits.Any() ? "" : "0.1.0";

        return
            $"Segment {Id,-3} {YoungestCommit.CommitId.ShortSha,7} -> {OldestCommit.CommitId.ShortSha,-7}   {commitsCount,5}    {ApiChangeFlags}   {release}";
    }

    private ApiChangeFlags GetApiChanges()
    {
        if (_bumps != null)
        {
            return _bumps;
        }

        var bumps = new ApiChangeFlags();
        foreach (var commit in _commits.Where(commit => !commit.IsARelease || commit.IsAWaypoint))
        {
            bumps.Aggregate(commit.MessageMetadata.ApiChangeFlags);
            bumps.Aggregate(commit.TagMetadata.ChangeFlags);
        }

        _bumps = bumps;
        return bumps;
    }

    private GitSegment SplitAt(Commit commit, IGitSegmentFactory segmentFactory)
    {
        var index = _commits.IndexOf(commit);
        if (index < 0)
        {
            throw new Git2SemVerInvalidOperationException("Cannot split a segment that does not contain the commit.");
        }

        if (index == 0)
        {
            throw new Git2SemVerInvalidOperationException("Cannot split a segment at its first (youngest) commit.");
        }

        using (_logger.EnterLogScope())
        {
            var keepCommits = _commits.Take(index).ToList();
            var olderSegmentCommits = _commits.Skip(index).Take(_commits.Count - index).ToArray();
            _commits.Clear();
            _commits.AddRange(keepCommits);

            var olderSegment = segmentFactory.Create(olderSegmentCommits);
            _logger.LogTrace("Split out new segment {2} from segment {0} at commit {1}.",
                             Id, commit.CommitId.ShortSha,
                             olderSegment.Id);

            return olderSegment;
        }
    }
}