using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.MSBuild.Tasking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;

public sealed class VersioningContext : IVersioningContext
{
    internal VersioningContext(IVersionGeneratorInputs inputs, IVersionOutputs outputs,
                               IBuildHost host,
                               ILogger logger)
    {
        Inputs = inputs;
        Outputs = outputs;
        Host = host;
        Logger = logger;
        MsBuildGlobalProperties = new MSBuildGlobalProperties(inputs.BuildEngine9);
        Instance = this;
    }

    public IBuildHost Host { get; }

    public IVersionGeneratorInputs Inputs { get; }

    public static IVersioningContext? Instance { get; private set; }

    public ILogger Logger { get; }

    public MSBuildGlobalProperties MsBuildGlobalProperties { get; }

    public IVersionOutputs Outputs { get; }
}