using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using Semver;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

internal sealed class VersionHistorySegment
{
    private static int _nextId = 1;
    private readonly List<Commit> _commits = [];
    private readonly ILogger _logger;
    private readonly IList<VersionHistorySegment> _older = [];
    private readonly IList<VersionHistorySegment> _younger = [];
    private ApiChanges? _bumps;

    private VersionHistorySegment(List<Commit> commits, ILogger logger) : this(logger)
    {
        _commits.AddRange(commits);
    }

    internal static void Reset()
    {
        _nextId = 1;
    }

    private VersionHistorySegment(ILogger logger)
    {
        _logger = logger;
        Id = _nextId++;
    }

    public ApiChanges Bumps => GetVersionBumps();

    public IReadOnlyList<Commit> Commits => _commits.ToList();

    /// <summary>
    /// First (oldest) commit in the segment.
    /// </summary>
    public Commit FirstCommit => _commits.Last();

    public IReadOnlyList<VersionHistorySegment> From => _older.ToList();

    public int Id { get; }

    /// <summary>
    /// Last (youngest) commit in the segment.
    /// </summary>
    public Commit LastCommit => _commits[0];

    public SemVersion? TaggedReleasedVersion => _commits.Count != 0 ? FirstCommit.ReleasedVersion : null;

    public bool IsLeaf => TaggedReleasedVersion != null || From.Count == 0;

    public IReadOnlyList<VersionHistorySegment> To => _younger.ToList();

    public void Append(Commit commit)
    {
        if (_commits.Count > 0 && FirstCommit.Parents.All(x => x.Id != commit.CommitId.Id))
        {
            throw new InvalidOperationException($"Cannot append {commit.CommitId.ObfuscatedSha} as it is not connected to segment's first (oldest) commit.");
        }

        _bumps = null;
        _commits.Add(commit);
        _logger.LogTrace("Commit {0} added to segment {1}.", commit.CommitId.ObfuscatedSha, Id);
    }

    public VersionHistorySegment? BranchedFrom(VersionHistorySegment branchSegment, Commit commit)
    {
        _logger.LogDebug("Commit {0} in segment {1} branches to segment {2}:", commit.CommitId.ObfuscatedSha, Id, branchSegment.Id);
        using (_logger.EnterLogScope())
        {
            if (commit.CommitId.Equals(LastCommit.CommitId))
            {
                _logger.LogTrace("Commit {0} is last (youngest) commit in segment {1}. Link segments.", commit.CommitId.ObfuscatedSha, Id);
                LinkToYounger(branchSegment);
                return null;
            }

            _bumps = null;
            var fromSegment = SplitSegmentAt(commit);
            fromSegment.LinkToYounger(branchSegment);
            return fromSegment;
        }
    }

    public static VersionHistorySegment CreateHeadSegment(ILogger logger)
    {
        return new VersionHistorySegment(logger);
    }

    /// <summary>
    ///     Create a (younger) segment that this segment links to.
    /// </summary>
    public VersionHistorySegment CreateMergedSegment()
    {
        var fromBranch = new VersionHistorySegment(_logger);
        fromBranch.LinkToYounger(this);
        return fromBranch;
    }

    public override string ToString()
    {
        var toSegments = !To.Any()
            ? "To: none (head)"
            : $"To: {string.Join(",", To.Select(x => x.Id))}";

        var fromSegments = From.Any()
            ? $"From: {string.Join(",", From.Select(x => x.Id))}"
            : "From: none";

        var commitsCount = $"({_commits.Count})";

        var release = TaggedReleasedVersion != null ? TaggedReleasedVersion.ToString() : "";

        return
            $"Segment {Id,-3} {LastCommit.CommitId.ObfuscatedSha} -> {FirstCommit.CommitId.ObfuscatedSha}  {commitsCount,5}  {Bumps.ToString() ?? "???"}  {toSegments,-16}  {fromSegments,-16}  {release}";
    }

    private ApiChanges GetVersionBumps()
    {
        if (_bumps != null)
        {
            return _bumps.Value;
        }

        var bumps = new ApiChanges();

        var parser = new ConventionalCommitParser(_logger);
        foreach (var commit in _commits)
        {
            var commitBumps = parser.Parse(commit);
            bumps.Aggregate(commitBumps);
        }

        _bumps = bumps;
        return bumps;
    }

    private void LinkToYounger(VersionHistorySegment toSegment)
    {
        _younger.Add(toSegment);
        toSegment._older.Add(this);
    }

    private VersionHistorySegment SplitSegmentAt(Commit commit)
    {
        var index = _commits.IndexOf(commit);
        if (index < 0)
        {
            throw new Git2SemVerInvalidOperationException("Cannot split a segment that does not contain the commit.");
        }

        using (_logger.EnterLogScope())
        {
            var keepCommits = _commits.Take(index).ToList();
            var newSegmentCommits = _commits.Skip(index).Take(_commits.Count - index).ToList();
            _commits.Clear();
            _commits.AddRange(keepCommits);

            var fromSegment = new VersionHistorySegment(newSegmentCommits, _logger);
            _logger.LogTrace("Split out new segment {2} from segment {0} at commit {1}.", 
                             Id, commit.CommitId.ObfuscatedSha,
                             fromSegment.Id);
            foreach (var olderSegment in _older)
            {
                fromSegment._older.Add(olderSegment);
            }
            _older.Clear();
            fromSegment.LinkToYounger(this);
            return fromSegment;
        }
    }
}