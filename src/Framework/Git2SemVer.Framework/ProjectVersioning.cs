﻿using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Persistence;


#pragma warning disable CA1859

namespace NoeticTools.Git2SemVer.Framework;

public sealed class ProjectVersioning : IDisposable
{
    private readonly IBuildHost _host;
    private readonly IVersionGeneratorInputs _inputs;
    private readonly ILogger _logger;
    private readonly IOutputsJsonIO _outputsCacheJsonFile;
    private readonly IVersioningEngine _versioningEngine;

    internal ProjectVersioning(
        IVersionGeneratorInputs inputs,
        IBuildHost host,
        IOutputsJsonIO outputsCacheJsonFile,
        IVersioningEngine versioningEngine,
        ILogger logger)
    {
        _inputs = inputs;
        _host = host;
        _outputsCacheJsonFile = outputsCacheJsonFile;
        _versioningEngine = versioningEngine;
        _logger = logger;
    }

    public void Dispose()
    {
        _versioningEngine.Dispose();
        _host.Dispose();
    }

    public IVersionOutputs Run()
    {
        try
        {
            var handlers = new Dictionary<VersioningMode, Func<IVersionOutputs>>
            {
                { VersioningMode.SolutionVersioningProject, PerformSolutionVersioningProjectVersioning },
                { VersioningMode.SolutionClientProject, PerformSolutionClientVersioning },
                { VersioningMode.StandAloneProject, PerformStandAloneProjectVersioning }
            };

            var outputs = handlers[_inputs.VersioningMode]();
            UpdateHostBuildLabel(outputs);
            return outputs;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception);
            throw;
        }
    }

    private string GetClientLastBuildNumber()
    {
        var localCache = _outputsCacheJsonFile.Load(_inputs.IntermediateOutputDirectory);
        if (localCache.IsValid)
        {
            return localCache.BuildNumber;
        }

        var shared = _outputsCacheJsonFile.Load(_inputs.SolutionSharedDirectory);
        return !shared.IsValid ? _host.BuildNumber : shared.BuildNumber;
    }

    private IVersionOutputs PerformSolutionClientVersioning()
    {
        _logger.LogTrace("Versioning mode: Client");

        var lastBuildNumber = GetClientLastBuildNumber();
        if (lastBuildNumber == _host.BuildNumber)
        {
            return _versioningEngine.PrebuildRun();
        }

        var output = _outputsCacheJsonFile.Load(_inputs.SolutionSharedDirectory);
        _outputsCacheJsonFile.Write(_inputs.IntermediateOutputDirectory, output);
        return output;
    }

    private IVersionOutputs PerformSolutionVersioningProjectVersioning()
    {
        _logger.LogTrace("Versioning mode: Solution");
        var output = _outputsCacheJsonFile.Load(_inputs.SolutionSharedDirectory);
        return !output.IsValid ? _versioningEngine.PrebuildRun() : output;
    }

    private IVersionOutputs PerformStandAloneProjectVersioning()
    {
        _logger.LogTrace("Versioning mode: Stand-alone project");
        return _versioningEngine.PrebuildRun();
    }

    private void UpdateHostBuildLabel(IVersionOutputs output)
    {
        if (_inputs.UpdateHostBuildLabel && output.BuildSystemVersion != null)
        {
            _host.SetBuildLabel(output.BuildSystemVersion.ToString());
        }
    }
}