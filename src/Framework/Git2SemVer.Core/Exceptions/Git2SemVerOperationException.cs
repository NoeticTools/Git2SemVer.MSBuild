using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerOperationException : Git2SemverExceptionBase
{
    public Git2SemVerOperationException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}