using System.Diagnostics;
using System.Text;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;


namespace NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;

internal sealed class GitSegmentsBuilder : IGitSegmentsBuilder
{
    private readonly Dictionary<CommitId, GitSegment> _commitsCache = new();
    private readonly IGitTool _gitTool;
    private readonly ILogger _logger;
    private readonly GitSegment _segment;
    private readonly IGitSegmentFactory _segmentFactory;
    private readonly Dictionary<int, GitSegment> _segments = [];

    private GitSegmentsBuilder(GitSegmentsBuilder parent)
    {
        _logger = parent._logger;
        _segments = parent._segments;
        _segmentFactory = parent._segmentFactory;
        _gitTool = parent._gitTool;
        _commitsCache = parent._commitsCache;
        _segment = _segmentFactory.Create();
        _segments.Add(_segment.Id, _segment);
    }

    public GitSegmentsBuilder(IGitTool gitTool, ILogger logger)
    {
        _gitTool = gitTool;
        _logger = logger;
        _segmentFactory = new GitSegmentFactory(logger);
        _segment = _segmentFactory.Create();
        _segments.Add(_segment.Id, _segment);
    }

    public ContributingCommits GetContributingCommits(Commit headCommit)
    {
        var stopwatch = Stopwatch.StartNew();
        FindSegmentsReachableFrom(headCommit);
        stopwatch.Stop();
        _logger.LogDebug($"Found {_segments.Count} segment{(_segments.Count == 1 ? "" : "s")} (in {stopwatch.Elapsed.TotalMilliseconds:F0} ms):");
        using (_logger.EnterLogScope())
        {
            _logger.LogDebug(GetFoundSegmentsReport());
        }

        return new ContributingCommits(_segments.Values.ToList(), headCommit, _gitTool.BranchName);
    }

    private void FindSegmentsReachableFrom(Commit commit)
    {
        while (NextCommit(commit) == SegmentWalkResult.Continue)
        {
            commit = _gitTool.Get(commit.Parents.First());
        }
    }

    private string GetFoundSegmentsReport()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Segment #      From -> To      Commits    Bumps  Release");
        foreach (var segment in _segments)
        {
            stringBuilder.AppendLine(segment.Value.ToString());
        }

        return stringBuilder.ToString().TrimEnd();
    }

    private SegmentWalkResult NextCommit(Commit commit)
    {
        if (_commitsCache.ContainsKey(commit.CommitId))
        {
            OnBranchFromExistingSegment(commit);
            return SegmentWalkResult.FoundStart;
        }

        _segment.Append(commit);
        _commitsCache.Add(commit.CommitId, _segment);

        if (commit.TagMetadata.IsARelease)
        {
            _logger.LogTrace("Found release {1} at commit '{0}'.",
                             commit.CommitId.ShortSha,
                             commit.TagMetadata.Version!.ToString());

            return SegmentWalkResult.FoundStart;
        }

        if (commit.TagMetadata.ReleaseType == ReleaseTypeId.ReleaseWaypoint)
        {
            _logger.LogTrace("Found release waypoint at commit '{0}'.",
                             commit.CommitId.ShortSha);
            return SegmentWalkResult.FoundStart;
        }

        if (commit.TagMetadata.ReleaseType == ReleaseTypeId.RootCommit)
        {
            _logger.LogTrace("Found that the repository's first commit '{0}' is reachable from the head commit without a released commit.",
                             commit.CommitId.ShortSha);
            return SegmentWalkResult.FoundStart;
        }

        if (commit.Parents.Length != 2)
        {
            return SegmentWalkResult.Continue;
        }

        OnMergeCommit(commit);
        return SegmentWalkResult.FoundStart;
    }

    private void NextCommitBeforeMerge(CommitId branchCommitId)
    {
        var parentCommit = _gitTool.Get(branchCommitId);

        if (_commitsCache.ContainsKey(parentCommit.CommitId))
        {
            OnBranchFromExistingSegment(parentCommit);
        }
        else
        {
            using (_logger.EnterLogScope())
            {
                var newSegmentVisitor = new GitSegmentsBuilder(this);
                newSegmentVisitor.FindSegmentsReachableFrom(parentCommit);
            }
        }
    }

    /// <summary>
    ///     Found commit which is merge point on existing segment.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Claves off new segment from an existing segment at a given "branched from" commit.
    ///     </para>
    ///     <code>
    ///        merge commit  |   |
    ///                     / \  | intersected segment (existing)
    ///                    /   \ |
    ///                   /     \|
    ///                  |       | branched from commit
    ///                  |       |
    /// segment 2 (new)  |       | split segment (split from intersected segment)
    /// </code>
    /// </remarks>
    private void OnBranchFromExistingSegment(Commit branchedFromCommit)
    {
        var intersectedSegment = _commitsCache[branchedFromCommit.CommitId];
        var splitSegment = intersectedSegment.BranchesFrom(_segment, branchedFromCommit, _segmentFactory);
        if (splitSegment == null)
        {
            return;
        }

        _segments.Add(splitSegment.Id, splitSegment);
        foreach (var segmentCommit in splitSegment.Commits)
        {
            _commitsCache[segmentCommit.CommitId] = splitSegment;
        }
    }

    private void OnMergeCommit(Commit mergeCommit)
    {
        var mergedBranchCommit = mergeCommit.Parents[0];
        var continuingBranchCommit = mergeCommit.Parents[1];
        using (_logger.EnterLogScope())
        {
            _logger.LogTrace($"Commit {mergeCommit.CommitId.ShortSha} is a merge commit.");
        }

        _logger.LogTrace("Continuing branch:");
        NextCommitBeforeMerge(continuingBranchCommit);

        _logger.LogTrace($"Commit {mergeCommit.CommitId.ShortSha} is a merge commit from branch commit {mergedBranchCommit.ShortSha}:");
        using (_logger.EnterLogScope())
        {
            NextCommitBeforeMerge(mergedBranchCommit);
        }
    }

    private enum SegmentWalkResult
    {
        Continue,
        FoundStart
    }
}