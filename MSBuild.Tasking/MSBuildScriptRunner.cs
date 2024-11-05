using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.Extensions.Primitives;
using NoeticTools.Common.Logging;
using Task = System.Threading.Tasks.Task;


// ReSharper disable MethodHasAsyncOverload

namespace NoeticTools.MSBuild.Tasking;

public sealed class MSBuildScriptRunner
{
    private readonly ILogger _logger;

    public MSBuildScriptRunner(ILogger logger)
    {
        _logger = logger;
    }

    public static IReadOnlyList<string> ReferencedAssemblies { get; private set; } = [];

    public async Task RunScript(object globalContext,
                                string scriptPath,
                                IReadOnlyList<Type> metadataReferences,
                                IList<Type> inMemoryTypes)
    {
        try
        {
            if (_logger.HasError)
            {
                return;
            }

            _logger.LogDebug($"Running script: {scriptPath}.");
            using (_logger.EnterLogScope())
            {
                using (var loader = new InteractiveAssemblyLoader())
                {
                    var globalsType = globalContext!.GetType();
                    foreach (var inMemoryType in inMemoryTypes)
                    {
                        loader.RegisterDependency(inMemoryType.Assembly);
                    }

                    var scriptContent = File.ReadAllText(scriptPath);
                    var scriptOptions = GetScriptOptions(metadataReferences);
                    scriptOptions.WithEmitDebugInformation(true);
                    var script = CSharpScript.Create<int>(scriptContent,
                                                          scriptOptions,
                                                          assemblyLoader: loader,
                                                          globalsType: globalsType);
                    await script.RunAsync(globalContext);
                }
            }
        }
        catch (CompilationErrorException exception)
        {
            _logger.LogError($"Error compiling script {scriptPath}. \nSource: {exception.Source}");
            _logger.LogError(exception);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception);
        }
    }

    internal static ScriptOptions GetScriptOptions(IReadOnlyList<Type> metadataReferences)
    {
        var types = new List<Type>(
        [
            typeof(MSBuildScriptRunner),
            typeof(MSBuildGlobalProperties),
            typeof(string),
            typeof(File),
            typeof(Regex),
            typeof(BigInteger),
            typeof(Enumerable),
            typeof(ISerializable),
            typeof(NumberStyles),
            typeof(MethodImplOptions),
            typeof(LayoutKind),
            typeof(StringSegment),
            typeof(ExcludeFromCodeCoverageAttribute),
            typeof(Stopwatch),
            typeof(ICollection),
            typeof(ICollection<>)
        ]);
        types.AddRange(metadataReferences);

        types = types.Distinct().ToList();
        var assemblies = types.Select(x => x.Assembly).Distinct().ToList();
        var namespaces = types.Select(x => x.Namespace).Distinct();
        ReferencedAssemblies = assemblies.Select(x => x.GetName().Name).ToList();

        var scriptOptions = ScriptOptions.Default;
        scriptOptions = scriptOptions.AddReferences(assemblies);
        scriptOptions = scriptOptions.AddImports(namespaces);
        return scriptOptions;
    }
}