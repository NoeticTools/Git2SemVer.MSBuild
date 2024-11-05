using Microsoft.Build.Framework;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using ILogger = NoeticTools.Common.Logging.ILogger;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global


namespace NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;

public class VersionGeneratorInputsStub : IVersionGeneratorInputs
{
    public string BranchMaturityPattern { get; set; } = "";

    public string BuildContext { get; set; } = string.Empty;

    public IBuildEngine9 BuildEngine9 { get; set; } = new BuildEngine9Stub(new Dictionary<string, string>());

    public string BuildNumber { get; set; } = "";

    public string BuildScriptPath { get; set; } = "";

    public string HostType { get; set; } = "";

    public string IntermediateOutputDirectory { get; } = "";

    public VersioningMode VersioningMode { get; set; }

    public bool? RunScript { get; set; }

    public string ScriptArgs { get; set; } = "";

    public string SolutionSharedDirectory { get; set; } = "";

    public string SolutionSharedVersioningPropsFile { get; set; } = "";

    public bool UpdateHostBuildLabel { get; set; }

    public string Version { get; set; } = "";

    public string VersionSuffix { get; set; } = "";

    public string WorkingDirectory { get; set; } = "";

    public string BuildIdFormat { get; } = "";

    public bool Validate(ILogger logger)
    {
        return true;
    }
}