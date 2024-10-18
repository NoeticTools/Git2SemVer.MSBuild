using System.Collections.Immutable;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

internal sealed class HistoryPaths : IHistoryPaths
{
    private readonly IReadOnlyList<VersionHistorySegment> _segments;

    public HistoryPaths(IReadOnlyList<VersionHistoryPath> paths,
                        IReadOnlyList<VersionHistorySegment> segments)
    {
        _segments = segments;
        Paths = paths.ToSortedSet();
        BestPath = Paths.First();
        HeadCommit = BestPath.HeadCommit;
    }

    public IVersionHistoryPath BestPath { get; }

    public Commit HeadCommit { get; }

    public ImmutableSortedSet<VersionHistoryPath> Paths { get; }

    public string GetReport()
    {
        return $"""
                Git history paths to last releases:
                
                  {_segments.Count} Segments:
                {string.Join("\n", _segments.Select(x => $"    {x}"))}
                
                  {Paths.Count} Paths:
                {string.Join("\n", Paths.Select(x => $"    {x}"))}
                
                  Walked {NumberOfCommits} commits
                """;
    }

    private int NumberOfCommits => _segments.Sum(x => x.Commits.Count);
}