using NoeticTools.Git2SemVer.MSBuild.Versioning;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;

internal class TaskOutputsStub : IVersionOutputs
{
    public Version? AssemblyVersion { get; set; }

    public string BuildContext { get; set; } = "";

    public string BuildNumber { get; set; } = "";

    public SemVersion? BuildSystemVersion { get; set; }

    public Version? FileVersion { get; set; }

    public GitOutputs Git { get; } = null!;

    public SemVersion? InformationalVersion { get; set; }

    public bool IsInInitialDevelopment { get; set; }

    public string Output1 { get; set; } = "";

    public string Output2 { get; set; } = "";

    public SemVersion? PackageVersion { get; set; }

    public string PrereleaseLabel { get; set; } = "";

    public SemVersion? Version { get; set; }

    public void SetAllVersionPropertiesFrom(SemVersion informationalVersion, string buildNumber, string buildContext)
    {
        throw new NotImplementedException();
    }
}