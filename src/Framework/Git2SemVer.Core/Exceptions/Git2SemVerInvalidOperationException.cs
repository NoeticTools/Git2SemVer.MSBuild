using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerInvalidOperationException : Git2SemverExceptionBase
{
    public Git2SemVerInvalidOperationException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerInvalidOperationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}