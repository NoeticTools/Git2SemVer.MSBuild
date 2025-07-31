// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.Core.Diagnostics;

//[DiagnosticCode]
public sealed class GSV00X : DiagnosticCodeBase
{
    public GSV00X(string sampleArg)
        : base(9999,
               "Versioning",
               """
               This occurs when ...
               """,
               """
               If there ...
               """,
               "The required build script file '{0}' was not found.",
               sampleArg)
    {
    }
}