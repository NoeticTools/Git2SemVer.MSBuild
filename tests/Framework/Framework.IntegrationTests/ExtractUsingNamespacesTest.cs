using NoeticTools.Git2SemVer.Framework.Framework;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;


#pragma warning disable NUnit2046

#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.Framework.IntegrationTests;

[TestFixture]
public class ExtractUsingNamespacesTest
{
    [Test]
    public void ExtractCsxRunnerOptions()
    {
        var options = CSharpScriptRunner.GetScriptOptions(Git2SemVerScriptRunner.MetadataReferences);

        Assert.That(options, Is.Not.Null);
        Assert.That(options.Imports.Length, Is.GreaterThan(10));
        Assert.That(CSharpScriptRunner.ReferencedAssemblies.Count, Is.GreaterThan(5));

        // Provide a list to copy and paste to documentation
        TestContext.Out.WriteLine("Namespaces:");
        foreach (var @namespace in options.Imports)
        {
            TestContext.Out.WriteLine("  " + @namespace);
        }

        // Provide a list to copy and paste to documentation
        TestContext.Out.WriteLine("Referenced assemblies:");
        foreach (var assemblyName in CSharpScriptRunner.ReferencedAssemblies)
        {
            TestContext.Out.WriteLine("  " + assemblyName);
        }
    }
}