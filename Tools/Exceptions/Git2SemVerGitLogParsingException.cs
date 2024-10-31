namespace NoeticTools.Common.Exceptions;

public class Git2SemVerGitLogParsingException : Git2SemverExceptionBase
{
    public Git2SemVerGitLogParsingException(string message) : base(message)
    {
    }

    public Git2SemVerGitLogParsingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}