using NoeticTools.Git2SemVer.Core.Diagnostics;


namespace NoeticTools.DiagnosticCodesDocBuilder;

internal static class DiagCodeInfoProvider
{
    public static IReadOnlyList<DiagnosticCodeBase> Get()
    {
        var assembly = typeof(DiagnosticCodeBase).Assembly;
        var types = assembly.GetTypes()
                            .Where(t => t.GetCustomAttributes(typeof(DiagnosticCodeAttribute), false).Length > 0)
                            .ToArray();

        Console.WriteLine($"Generating {types.Length} diagnostic code pages.");

        var diagCodes = new List<DiagnosticCodeBase>();

        foreach (var type in types)
        {
            var constructor = type.GetConstructors().First();

            var valueArgs = new object[constructor.GetParameters().Length]!;
            for (var index = 0; index < valueArgs.Length; index++)
            {
                valueArgs[index] = "value";
            }

            var instance = (DiagnosticCodeBase)constructor.Invoke(valueArgs);
            diagCodes.Add(instance);
        }

        return diagCodes;
    }
}