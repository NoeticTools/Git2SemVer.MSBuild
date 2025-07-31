using System.Diagnostics;


namespace NoeticTools.Git2SemVer.Core;

[DebuggerStepThrough]
public static class Ensure
{
    public static void NotNull(object argumentValue, string argumentName)
    {
        if (argumentValue == null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }
}