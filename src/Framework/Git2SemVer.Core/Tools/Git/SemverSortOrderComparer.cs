using Semver;


namespace NoeticTools.Git2SemVer.Core.Tools.Git;

#pragma warning disable CS1591
internal sealed class SemverSortOrderComparer : IComparer<SemVersion>
{
    public int Compare(SemVersion? x, SemVersion? y)
    {
        if (x == null)
        {
            throw new ArgumentNullException(nameof(x));
        }

        if (y == null)
        {
            throw new ArgumentNullException(nameof(y));
        }

        return x.CompareSortOrderTo(y);
    }
}