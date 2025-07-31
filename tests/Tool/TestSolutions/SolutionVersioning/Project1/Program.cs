using System.Diagnostics;
using System.Reflection;

namespace NoeticTools.TestProject1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Git2SemVer test project");
            Console.WriteLine();

            var assembly = typeof(Program).Assembly;
            var informationalVersionAttribute = GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly);
            var assemblyFileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine($"Assembly version:       {assembly.GetName().Version}");
            Console.WriteLine($"File version:           {assemblyFileInfo.FileVersion}");
            Console.WriteLine($"Informational version:  {informationalVersionAttribute!.InformationalVersion}");
            Console.WriteLine($"Product version:        {assemblyFileInfo.ProductVersion}");
        }

        private static T GetCustomAttribute<T>(Assembly assembly) where T : class
        {
            return (Attribute.GetCustomAttribute(assembly, typeof(T)) as T)!;
        }
    }
}
