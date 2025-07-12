// Copied from semver NuGet package to resolve runtime build issues.

using System.Runtime.CompilerServices;


namespace NoeticTools.Git2SemVer.Core;

public static class EnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> values)
    {
        return values.ToList().AsReadOnly();
    }
}