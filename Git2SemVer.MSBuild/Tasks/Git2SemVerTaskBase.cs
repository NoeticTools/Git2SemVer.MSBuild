using Microsoft.Build.Framework;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using Task = Microsoft.Build.Utilities.Task;


// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

public abstract class Git2SemVerTaskBase : Task
{
    [Output]
    public string Output_AssemblyVersion { get; set; } = "";

    [Output]
    public int Output_CommitsSinceLastRelease { get; set; }

    [Output]
    public string Output_FileVersion { get; set; } = "";

    [Output]
    public string Output_InformationalVersion { get; set; } = "";

    [Output]
    public bool Output_IsRelease { get; set; }

    [Output]
    public string Output_LastReleaseCommitId { get; set; } = "";

    [Output]
    public string Output_LastReleaseVersion { get; set; } = "";

    [Output]
    public string Output_PackageVersion { get; set; } = "";

    [Output]
    public string Output_Version { get; set; } = "";

    [Output]
    public string Output_VersionPrefix { get; set; } = "";

    [Output]
    public string Output_VersionSuffix { get; set; } = "";

    [Output]
    public string Output1 { get; set; } = "";

    [Output]
    public string Output2 { get; set; } = "";

    protected void SetOutputs(IVersionOutputs outputs)
    {
        Output_Version = outputs.Version?.ToString() ?? "";
        Output_VersionSuffix = outputs.Version?.Prerelease ?? "";
        Output_VersionPrefix = outputs.Version?.WithoutPrerelease().WithoutMetadata().ToString() ?? "";
        Output_AssemblyVersion = outputs.AssemblyVersion?.ToString() ?? "";
        Output_FileVersion = outputs.FileVersion?.ToString() ?? "";
        Output_InformationalVersion = outputs.InformationalVersion?.ToString() ?? "";
        Output_PackageVersion = outputs.PackageVersion?.ToString() ?? "";
        Output1 = outputs.Output1;
        Output2 = outputs.Output2;
        Output_CommitsSinceLastRelease = outputs.Git.CommitsSinceLastRelease;
        Output_IsRelease = outputs.Version?.IsRelease ?? false;

        Output_LastReleaseCommitId = outputs.Git.LastReleaseCommit?.CommitId.Id ?? "";
        Output_LastReleaseVersion = outputs.Git.LastReleaseVersion?.ToString() ?? "";
    }
}