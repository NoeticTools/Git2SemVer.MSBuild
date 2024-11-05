namespace NoeticTools.Common.Exceptions;

public abstract class Git2SemverExceptionBase : Exception
{
    protected Git2SemverExceptionBase(string message) : base(message)
    {
    }

    protected Git2SemverExceptionBase(string message, Exception innerException) : base(message, innerException)
    {
    }
}