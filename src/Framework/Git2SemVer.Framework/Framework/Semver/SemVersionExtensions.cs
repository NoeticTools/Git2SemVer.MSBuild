using NoeticTools.Git2SemVer.Core.ConventionCommits;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Framework.Semver;

public static class SemVersionExtensions
{
    /// <summary>
    ///     Bump version according to Semantic Versioning specification.
    /// </summary>
    public static SemVersion Bump(this SemVersion lastReleased, ApiChangeFlags changeFlags)
    {
        if (changeFlags.BreakingChange)
        {
            return new SemVersion(lastReleased.Major + 1, 0, 0);
        }

        if (changeFlags.FunctionalityChange)
        {
            return new SemVersion(lastReleased.Major, lastReleased.Minor + 1, 0);
        }

        return new SemVersion(lastReleased.Major, lastReleased.Minor, lastReleased.Patch + 1);
    }
}