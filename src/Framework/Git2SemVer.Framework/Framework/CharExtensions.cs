using System.Runtime.CompilerServices;


namespace NoeticTools.Git2SemVer.Framework.Framework;

public static class CharExtensions
{
    /// <summary>
    ///     Is this character an ASCII digit '0' through '9'
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(this char c)
    {
        return c is >= '0' and <= '9';
    }
}