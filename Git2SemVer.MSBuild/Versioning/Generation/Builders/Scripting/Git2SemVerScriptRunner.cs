using System.Diagnostics;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.DotnetCli;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Tools.CI;
using NoeticTools.MSBuild.Tasking;
using NuGet.Versioning;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;

public sealed class Git2SemVerScriptRunner
{
    private readonly IBuildHost _host;
    private readonly MSBuildScriptRunner _innerScriptRunner;
    private readonly IVersionGeneratorInputs _inputs;
    private readonly ILogger _logger;
    private readonly IVersionOutputs _outputs;

    public Git2SemVerScriptRunner(MSBuildScriptRunner innerScriptRunner,
                                  IBuildHost host,
                                  IVersionGeneratorInputs inputs,
                                  IVersionOutputs outputs,
                                  ILogger logger)
    {
        _inputs = inputs;
        _outputs = outputs;
        _innerScriptRunner = innerScriptRunner;
        _host = host;
        _logger = logger;
    }

    internal static IReadOnlyList<Type> MetadataReferences { get; } = new[]
    {
        typeof(DotNetTool),
        typeof(UncontrolledHost),
        typeof(IBuildHost),
        typeof(GitTool),
        typeof(SemVersion),
        typeof(NuGetVersion),
        typeof(VersioningContext),
        typeof(ILogger)
    };

    internal async Task RunScript()
    {
        if (_inputs.RunScript == false)
        {
            _logger.LogDebug("RunScript option is not false. Script not run.");
            return;
        }

        if (_inputs.RunScript == null && !File.Exists(_inputs.BuildScriptPath))
        {
            _logger.LogDebug($"Script not found. BuildScriptPath is '{_inputs.BuildScriptPath}'.");
            return;
        }

        if (!_inputs.Validate(_logger))
        {
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        var inMemoryTypes = new List<Type>(new[]
        {
            typeof(VersioningContext),
            typeof(ILogger),
            typeof(SemVersion)
        });

        var context = new VersioningContext(_inputs, _outputs, _host, _logger);
        await _innerScriptRunner.RunScript(context, _inputs.BuildScriptPath, MetadataReferences, inMemoryTypes);

        stopwatch.Stop();
        _logger.LogInfo($"Script run completed. (in {stopwatch.Elapsed.TotalSeconds:F1} sec)");
        _host.ReportBuildStatistic("Git2SemVer_Script_Seconds", stopwatch.Elapsed.TotalSeconds);
    }
}