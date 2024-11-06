using System.Diagnostics;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Persistence;


#pragma warning disable CA1859

namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

internal class VersionGenerator
{
    private readonly IDefaultVersionBuilderFactory _defaultVersionBuilderFactory;
    private readonly IGeneratedOutputsJsonFile _generatedOutputsJsonFile;
    private readonly IGeneratedOutputsPropFile _generatedOutputsPropFile;
    private readonly IGitHistoryPathsFinder _gitPathsFinder;
    private readonly IGitTool _gitTool;
    private readonly IBuildHost _host;
    private readonly IVersionGeneratorInputs _inputs;
    private readonly ILogger _logger;
    private readonly IVersionBuilder _scriptBuilder;

    public VersionGenerator(IVersionGeneratorInputs inputs, 
                            IBuildHost host,
                            IGeneratedOutputsJsonFile generatedOutputsJsonFile,
                            IGeneratedOutputsPropFile generatedOutputsPropFile,
                            IGitTool gitTool,
                            IGitHistoryPathsFinder gitPathsFinder,
                            IDefaultVersionBuilderFactory defaultVersionBuilderFactory,
                            IVersionBuilder scriptBuilder,
                            ILogger logger)
    {
        _inputs = inputs;
        _host = host;
        _generatedOutputsJsonFile = generatedOutputsJsonFile;
        _generatedOutputsPropFile = generatedOutputsPropFile;
        _gitTool = gitTool;
        _gitPathsFinder = gitPathsFinder;
        _defaultVersionBuilderFactory = defaultVersionBuilderFactory;
        _scriptBuilder = scriptBuilder;
        _logger = logger;
    }

    public IVersionOutputs Run()
    {
        try
        {
            if (_inputs.VersioningMode != VersioningMode.SolutionClientProject)
            {
                var output = GenerateVersion();
                if (_inputs.UpdateHostBuildLabel && output.BuildSystemVersion != null)
                {
                    _host.SetBuildLabel(output.BuildSystemVersion.ToString());
                }

                return output;
            }

            var localCache = _generatedOutputsJsonFile.Load(_inputs.IntermediateOutputDirectory);
            if (localCache.BuildNumber == _host.BuildNumber)
            {
                return GenerateVersion();
            }

            // Copy solution shared file to local outputs file
            var generatedOutputs = _generatedOutputsJsonFile.Load(_inputs.SolutionSharedDirectory);
            WriteOutputsToFile(_inputs.IntermediateOutputDirectory, generatedOutputs);
            return generatedOutputs;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception);
            throw;
        }
    }

    private IVersionOutputs GenerateVersion()
    {
        var stopwatch = Stopwatch.StartNew();

        _host.BumpBuildNumber();
        var historyPaths = _gitPathsFinder.FindPathsToHead();
        var outputs = new VersionOutputs(new GitOutputs(_gitTool, historyPaths));

        RunBuilders(outputs, historyPaths);
        SaveGeneratedVersions(outputs);

        stopwatch.Stop();
        _host.ReportBuildStatistic("Git2SemVerRunTime_sec", stopwatch.Elapsed.TotalSeconds);
        _logger.LogInfo($"Git2SemVer calculated version: {outputs.InformationalVersion}  ({stopwatch.Elapsed.TotalSeconds:F1} sec))");

        return outputs;
    }

    private void RunBuilders(VersionOutputs outputs, HistoryPaths historyPaths)
    {
        _defaultVersionBuilderFactory.Create(historyPaths).Build(_host, _inputs, outputs);
        _scriptBuilder.Build(_host, _inputs, outputs);
    }

    private void SaveGeneratedVersions(VersionOutputs outputs)
    {
        WriteOutputsToFile(_inputs.IntermediateOutputDirectory, outputs);
        if (_inputs.VersioningMode != VersioningMode.StandAloneProject)
        {
            WriteOutputsToFile(_inputs.SolutionSharedDirectory, outputs);
        }
    }

    private void WriteOutputsToFile(string outputDirectory, VersionOutputs generatedOutputs)
    {
        _generatedOutputsJsonFile.Write(outputDirectory, generatedOutputs);
        _generatedOutputsPropFile.Write(outputDirectory, generatedOutputs);
    }
}