using System.Diagnostics.CodeAnalysis;


namespace NoeticTools.Git2SemVer.Core.Interops.DotNet;

[RegisterSingleton]
[ExcludeFromCodeCoverage]
public sealed class FileInterop : IFiles
{
    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }
}