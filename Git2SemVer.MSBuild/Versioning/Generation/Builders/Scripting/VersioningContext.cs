﻿using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.MSBuild.Tasking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;

public sealed class VersioningContext : IVersioningContext
{
    internal VersioningContext(IVersionGeneratorInputs inputs,
                               IVersionOutputs outputs,
                               IBuildHost host,
                               IGitTool gitTool,
                               ILogger logger)
    {
        Inputs = inputs;
        Outputs = outputs;
        Host = host;
        Logger = logger;
        MsBuildGlobalProperties = new MSBuildGlobalProperties(inputs.BuildEngine9);
        Instance = this;
        Git = gitTool;
    }

    public IBuildHost Host { get; }

    public IGitTool Git { get; }

    public IVersionGeneratorInputs Inputs { get; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public static IVersioningContext? Instance { get; private set; }

    public ILogger Logger { get; }

    public MSBuildGlobalProperties MsBuildGlobalProperties { get; }

    public IVersionOutputs Outputs { get; }
}