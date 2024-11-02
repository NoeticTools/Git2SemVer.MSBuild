using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;


#pragma warning disable NUnit2045

namespace NoeticTools.MSBuild.Tasking.Tests;

[TestFixture]
public class ExtractUsingNamespacesTest
{
    [Test]
    public void ExtractCsxRunnerOptions()
    {
        var options = MSBuildScriptRunner.GetScriptOptions(Git2SemVerScriptRunner.MetadataReferences);

        Assert.That(options, Is.Not.Null);
        Assert.That(options.Imports.Length, Is.GreaterThan(10));
        Assert.That(MSBuildScriptRunner.ReferencedAssemblies.Count, Is.GreaterThan(5));

        // Provide a list to copy and paste to documentation
        TestContext.Progress.WriteLine("\nNamespaces:\n");
        foreach (var @namespace in options.Imports)
        {
            TestContext.Progress.WriteLine(@namespace);
        }

        // Provide a list to copy and paste to documentation
        TestContext.Progress.WriteLine("\nReferenced assemblies:\n");
        foreach (var assemblyName in MSBuildScriptRunner.ReferencedAssemblies)
        {
            TestContext.Progress.WriteLine(assemblyName);
        }
    }
}