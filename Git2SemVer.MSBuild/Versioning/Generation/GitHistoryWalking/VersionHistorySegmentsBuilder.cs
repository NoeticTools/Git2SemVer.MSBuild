using System.Diagnostics;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

internal sealed class VersionHistorySegmentsBuilder
{
    private readonly ICommitsRepository _commits;
    private readonly Dictionary<CommitId, VersionHistorySegment> _commitsCache = new();
    private readonly ILogger _logger;
    private readonly VersionHistorySegment _segment;
    private readonly Dictionary<int, VersionHistorySegment> _segments = [];

    private VersionHistorySegmentsBuilder(VersionHistorySegment segment, VersionHistorySegmentsBuilder parent)
    {
        _logger = parent._logger;
        _segments = parent._segments;
        _commits = parent._commits;
        _commitsCache = parent._commitsCache;
        _segment = segment;
        _segments.Add(segment.Id, segment);
    }

    public VersionHistorySegmentsBuilder(ICommitsRepository commits, ILogger logger)
    {
        _commits = commits;
        _logger = logger;
        _segment = VersionHistorySegment.CreateHeadSegment(logger);
        _segments.Add(_segment.Id, _segment);
    }

    public IReadOnlyList<VersionHistorySegment> BuildTo(Commit commit)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogDebug("Finding git path segments to last releases.");
        BuildSegmentsTo(commit);

        stopwatch.Stop();
        _logger.LogDebug($"Found {_segments.Count} segments ({stopwatch.ElapsedMilliseconds}ms)");

        using (_logger.EnterLogScope())
        {
            _logger.LogDebug("Segment #   Commits (count)      Bumps  To segments       From segments    Release");
            foreach (var segment in _segments)
            {
                _logger.LogDebug(segment.Value.ToString());
            }
        }

        return _segments.Values.ToList();
    }

    private void BuildSegmentsTo(Commit commit)
    {
        using (_logger.EnterLogScope())
        {
            while (NextCommit(commit) == SegmentWalkResult.Continue)
            {
                commit = _commits.Get(commit.Parents.First());
            }
        }
    }

    private void OnBranchFromExistingSegment(Commit commit)
    {
        var intersectingSegment = _commitsCache[commit.CommitId];
        var branchedFromSegment = intersectingSegment.BranchedFrom(_segment, commit);
        if (branchedFromSegment == null)
        {
            return;
        }

        _segments.Add(branchedFromSegment.Id, branchedFromSegment);
        foreach (var segmentCommit in branchedFromSegment.Commits)
        {
            _commitsCache[segmentCommit.CommitId] = branchedFromSegment;
        }
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

        if (commit.ReleasedVersion != null)
        {
            using (_logger.EnterLogScope())
            {
                _logger.LogTrace("Commit {0} has release tag '{1}'.", commit.CommitId.ObfuscatedSha, commit.ReleasedVersion.ToString());
            }
            return SegmentWalkResult.FoundStart;
        }

        var parents = commit.Parents.ToList();
        if (!parents.Any())
        {
            _logger.LogTrace("Commit {0} is the first (initial) commit in the repository (defaults to 0.1.0).", commit.CommitId.ObfuscatedSha);
            return SegmentWalkResult.FoundStart;
        }

        if (parents.Count == 2)
        {
            OnMergeCommit(commit);
            return SegmentWalkResult.FoundStart;
        }

        return SegmentWalkResult.Continue;
    }

    private void OnMergeCommit(Commit commit)
    {
        var mergedBranchCommit = commit.Parents[0];
        var continuingBranchCommit = commit.Parents[1];
        using (_logger.EnterLogScope())
        {
            _logger.LogDebug($"Commit {commit.CommitId.ObfuscatedSha} is a merge commit.");
        }

        _logger.LogTrace($"Continuing branch:");
        NextCommitBeforeMerge(continuingBranchCommit);

        _logger.LogDebug($"Commit {commit.CommitId.ObfuscatedSha} is a merge commit from branch commit {mergedBranchCommit.ObfuscatedSha}:");
        using (_logger.EnterLogScope())
        {
            NextCommitBeforeMerge(mergedBranchCommit);
        }
    }

    private void NextCommitBeforeMerge(CommitId parent)
    {
        var parentCommit = _commits.Get(parent);

        if (_commitsCache.ContainsKey(parentCommit.CommitId))
        {
            OnBranchFromExistingSegment(parentCommit);
        }
        else
        {
            var newSegmentVisitor = new VersionHistorySegmentsBuilder(_segment.CreateMergedSegment(), this);
            newSegmentVisitor.BuildSegmentsTo(parentCommit);
        }
    }

    private enum SegmentWalkResult
    {
        Unknown = 0,
        Continue,
        FoundStart
    }
}