using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerRepositoryException : Git2SemverExceptionBase
{
    public Git2SemVerRepositoryException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}