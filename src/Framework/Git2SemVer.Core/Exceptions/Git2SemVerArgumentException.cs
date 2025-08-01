using System.Diagnostics.CodeAnalysis;


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

    public static void ThrowIf(object? value, string argumentName, Func<object?, bool> predicate)
    {
        if (predicate(value))
        {
            throw new Git2SemVerArgumentException($"The argument {argumentName} failed to meet required predicate.");
        }
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
        if (string.IsNullOrEmpty(value))
        {
            throw new Git2SemVerArgumentException($"The string argument {argumentName} is required to a non-empty string.");
        }
    }
}