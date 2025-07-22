using System.Text;
using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.DiagnosticCodesDocBuilder.DocFx;

internal sealed class DiagCodesContentBuilder(string docsPath)
{
    public void Build(IReadOnlyList<DiagnosticCodeBase> diagCodes)
    {
        CreateContentFiles(diagCodes);
        CreateTocFile(diagCodes);
    }

    private void CreateContentFiles(IReadOnlyList<DiagnosticCodeBase> diagCodes)
    {
        foreach (var diagCode in diagCodes)
        {
            var fileName = $"{diagCode.Code}.md";
            var filePath = Path.Combine(docsPath, DocFolders.ErrorsAndWarnings, fileName);
            File.WriteAllText(filePath, $"""
                                         ---
                                         uid: {diagCode.Code}
                                         ---

                                         [![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)


                                         # {diagCode.Code} diagnostic code

                                         ## Message text

                                         ``{diagCode.Message}``

                                         ## Description

                                         {diagCode.Description}

                                         ## Resolution

                                         {diagCode.Resolution}
                                         """);
            Console.WriteLine($"  Created file: {Path.Combine(DocFolders.ErrorsAndWarnings, fileName)}");
        }
    }

    private void CreateTocFile(IReadOnlyList<DiagnosticCodeBase> diagCodes)
    {
        var stringBuilder = new StringBuilder();
        foreach (var diagCode in diagCodes.OrderBy(x => x.Code))
        {
            stringBuilder.AppendLine($"- name: {diagCode.Code}");
            stringBuilder.AppendLine($"  href: {diagCode.Code}.md");
        }

        var filePath = Path.Combine(docsPath, DocFolders.ErrorsAndWarnings, "toc.yml");
        File.WriteAllText(filePath, stringBuilder.ToString());
        Console.WriteLine($"  Created file: {Path.Combine(DocFolders.ErrorsAndWarnings, "toc.yml")}");
    }
}