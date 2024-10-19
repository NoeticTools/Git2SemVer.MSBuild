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
    private readonly IGeneratedOutputsFile _generatedOutputsFile;
    private readonly IGitHistoryPathsFinder _gitPathsFinder;
    private readonly IGitTool _gitTool;
    private readonly IBuildHost _host;
    private readonly IVersionGeneratorInputs _inputs;
    private readonly ILogger _logger;
    private readonly IVersionBuilder _scriptBuilder;

    public VersionGenerator(IVersionGeneratorInputs inputs, IBuildHost host,
                            IGeneratedOutputsFile generatedOutputsFile,
                            IGitTool gitTool,
                            IGitHistoryPathsFinder gitPathsFinder,
                            IDefaultVersionBuilderFactory defaultVersionBuilderFactory,
                            IVersionBuilder scriptBuilder,
                            ILogger logger)
    {
        _inputs = inputs;
        _host = host;
        _generatedOutputsFile = generatedOutputsFile;
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
            if (_inputs.Mode != VersioningModeEnum.SolutionClientProject)
            {
                var output = GenerateVersion();
                if (_inputs.UpdateHostBuildLabel && output.BuildSystemVersion != null)
                {
                    _host.SetBuildLabel(output.BuildSystemVersion.ToString());
                }

                return output;
            }

            var localCache = _generatedOutputsFile.Load(_inputs.IntermediateOutputDirectory);
            if (localCache.BuildNumber == _host.BuildNumber)
            {
                return GenerateVersion();
            }

            // Copy solution shared file to local outputs file
            var generatedOutputs = _generatedOutputsFile.Load(_inputs.SolutionSharedDirectory);
            _generatedOutputsFile.Save(_inputs.IntermediateOutputDirectory, generatedOutputs);
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
        SaveGeneratedVersion(outputs);

        stopwatch.Stop();
        _host.ReportBuildStatistic("Git2SemVerRunTime_sec", stopwatch.Elapsed.TotalSeconds);
        _logger.LogInfo($"Git2SemVer calculated version: {outputs.InformationalVersion}  ({stopwatch.Elapsed.TotalSeconds:F1} sec))");

        return outputs;
    }

    private void RunBuilders(VersionOutputs outputs, HistoryPaths historyPaths)
    {
        _defaultVersionBuilderFactory.Create(historyPaths, _host, _inputs, outputs).Build(_host, _inputs, outputs);
        _scriptBuilder.Build(_host, _inputs, outputs);
    }

    private void SaveGeneratedVersion(VersionOutputs outputs)
    {
        _generatedOutputsFile.Save(_inputs.IntermediateOutputDirectory, outputs);
        if (_inputs.Mode != VersioningModeEnum.StandAloneProject)
        {
            _generatedOutputsFile.Save(_inputs.SolutionSharedDirectory, outputs);
        }
    }
}