using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerInvalidFormatException : Git2SemverExceptionBase
{
    public Git2SemVerInvalidFormatException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerInvalidFormatException(string message, Exception innerException) : base(message, innerException)
    {
    }
}