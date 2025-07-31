using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.Git2SemVer.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class Git2SemVerDiagnosticCodeException : Git2SemverExceptionBase
{
    public Git2SemVerDiagnosticCodeException(DiagnosticCodeBase diagCode) : base(diagCode.MessageWithCode)
    {
        DiagCode = diagCode;
    }

    // ReSharper disable once UnusedMember.Global
    public Git2SemVerDiagnosticCodeException(DiagnosticCodeBase diagCode, Exception innerException) : base(diagCode.MessageWithCode, innerException)
    {
        DiagCode = diagCode;
    }

    public DiagnosticCodeBase DiagCode { get; }
}