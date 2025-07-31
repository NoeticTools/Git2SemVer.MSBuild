using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerGitLogParsingException : Git2SemverExceptionBase
{
    public Git2SemVerGitLogParsingException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerGitLogParsingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}