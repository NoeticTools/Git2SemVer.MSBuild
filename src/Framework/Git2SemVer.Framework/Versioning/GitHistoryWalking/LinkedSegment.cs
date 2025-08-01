using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;

#pragma warning disable CS1591
internal sealed class LinkedSegment(GitSegment segment)
{
    private readonly List<LinkedSegment> _linkedChildSegments = [];
    private readonly GitSegment _segment = segment;

    /// <summary>
    ///     Aggregated API changes from this segment up to all younger segments.
    /// </summary>
    public ApiChangeFlags ChangeFlags => _segment.ApiChangeFlags;

    public IReadOnlyList<Commit> Commits => _segment.Commits;

    public GitSegment Inner { get; } = segment;

    public IReadOnlyList<LinkedSegment> LinkedChildSegments => _linkedChildSegments;

    public Commit OldestCommit => _segment.OldestCommit;

    public Commit YoungestCommit => _segment.YoungestCommit;

    /// <summary>
    ///     Link to child (younger) segment.
    /// </summary>
    public void AddChild(LinkedSegment childSegment)
    {
        if (_linkedChildSegments.Contains(childSegment))
        {
            return;
        }

        if (_linkedChildSegments.Count == 2)
        {
            throw new Git2SemVerInvalidOperationException("A history segment may not have more than 2 child segments.");
        }

        _linkedChildSegments.Add(childSegment);
    }
}