using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerArgumentException : Git2SemverExceptionBase
{
    public Git2SemVerArgumentException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerArgumentException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(object? value, string argumentName)
    {
        if (value == null)
        {
            throw new Git2SemVerArgumentException($"The argument {argumentName} is required to not be null.");
        }
    }

    public static void ThrowIfNullOrEmpty(string value, string argumentName)
    {
        if (value == null || value.Length == 0)
        {
            throw new Git2SemVerArgumentException($"The string argument {argumentName} is required to a non-empty string.");
        }
    }
}