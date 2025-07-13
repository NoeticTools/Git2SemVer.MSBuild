using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerUnknownCommandOptionException : Git2SemverExceptionBase
{
    public Git2SemVerUnknownCommandOptionException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerUnknownCommandOptionException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}