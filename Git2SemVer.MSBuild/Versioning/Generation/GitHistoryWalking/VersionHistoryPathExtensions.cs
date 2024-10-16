using System.Collections.Immutable;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

#pragma warning disable CS1591

internal static class VersionHistoryPathExtensions
{
    internal static ImmutableSortedSet<VersionHistoryPath> ToSortedSet(this IReadOnlyList<VersionHistoryPath> paths)
    {
        return paths.ToImmutableSortedSet(new PathComparer());
    }

    private sealed class PathComparer : IComparer<VersionHistoryPath>
    {
        public int Compare(VersionHistoryPath? left, VersionHistoryPath? right)
        {
            var versionPrecedence = right!.Version.ComparePrecedenceTo(left!.Version);
            if (versionPrecedence == 0)
            {
                var sizePrecedence = left.CommitsSinceLastRelease - right.CommitsSinceLastRelease;
                if (sizePrecedence != 0)
                {
                    return sizePrecedence;
                }

                return left.Id - right.Id;
            }

            return versionPrecedence;
        }
    }
}