using System.Collections.Immutable;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

internal interface IHistoryPaths
{
    IVersionHistoryPath BestPath { get; }

    Commit HeadCommit { get; }

    ImmutableSortedSet<VersionHistoryPath> Paths { get; }

    string GetReport();
}