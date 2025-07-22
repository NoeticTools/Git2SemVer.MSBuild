using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerFileNotFoundException : Git2SemverExceptionBase
{
    public Git2SemVerFileNotFoundException(string message) : base(message)
    {
    }

    public Git2SemVerFileNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}