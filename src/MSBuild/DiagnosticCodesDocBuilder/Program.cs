using System.ComponentModel.DataAnnotations;
using NoeticTools.DiagnosticCodesDocBuilder.DocFx;
using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.DiagnosticCodesDocBuilder;

// ReSharper disable once ClassNeverInstantiated.Global
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Diagnostics DocFx pages builder.");

        var docsPath = args[0];
        if (!Directory.Exists(docsPath))
        {
            throw new ArgumentException($"Directory '{docsPath}' does not exist.");
        }

        Console.WriteLine($"Generate pages in directory: {docsPath}");

        var diagCodes = DiagCodeInfoProvider.Get();
        Validate(diagCodes);
        new DiagCodesContentBuilder(docsPath).Build(diagCodes);

        Console.WriteLine("Done");
    }

    private static void Validate(IReadOnlyList<DiagnosticCodeBase> diagCodes)
    {
        foreach (var diagCode in diagCodes)
        {
            var className = diagCode.GetType().Name;
            if (!className.Equals(diagCode.Code))
            {
                throw new ValidationException($"Diagnostic code class name '{className}' does not match the code '{diagCode.Code}'.");
            }
        }
    }
}