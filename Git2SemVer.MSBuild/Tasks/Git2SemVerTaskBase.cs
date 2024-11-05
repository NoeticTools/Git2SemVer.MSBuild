using Microsoft.Build.Framework;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using Task = Microsoft.Build.Utilities.Task;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global


// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.MSBuild.Tasks;

public abstract class Git2SemVerTaskBase : Task
{
    /// <summary>
    ///     MSBuild's <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#assemblyversion">AssemblyVersion</see> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    /// </remarks>
    [Output]
    public string AssemblyVersion { get; set; } = "";

    /// <summary>
    ///     The commits count (commit height) from the last release.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///         Made available to MSBuild for use by third party MSBuild tasks.
    ///     </para>
    ///     <para>
    ///         Git2SemVer does not use this value in versions generated.
    ///     </para>
    /// </remarks>
    [Output]
    public int CommitsSinceLastRelease { get; set; }

    /// <summary>
    ///     MSBuild's <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#fileversion">FileVersion</see> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    /// </remarks>
    [Output]
    public string FileVersion { get; set; } = "";

    /// <summary>
    ///     MSBuild's <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#informationalversion">InformationalVersion</see> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    /// </remarks>
    [Output]
    public string InformationalVersion { get; set; } = "";

    /// <summary>
    ///     True if the current build is a release build (build maturity).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///         Made available to MSBuild for use by third party MSBuild tasks.
    ///     </para>
    /// </remarks>
    [Output]
    public bool IsRelease { get; set; }

    /// <summary>
    ///     The found last release commit ID.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///         Made available to MSBuild for use by third party MSBuild tasks.
    ///     </para>
    /// </remarks>
    [Output]
    public string LastReleaseCommitId { get; set; } = "";

    /// <summary>
    ///     The found last released version.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///         Made available to MSBuild for use by third party MSBuild tasks.
    ///     </para>
    /// </remarks>
    [Output]
    public string LastReleaseVersion { get; set; } = "";

    /// <summary>
    ///     MSBuild's <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#packageversion">PackageVersion</see> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    /// </remarks>
    [Output]
    public string PackageVersion { get; set; } = "";

    /// <summary>
    ///     MSBuild's <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#version">Version</see> property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input and output.
    ///     </para>
    ///     <para>
    ///         Not used by Git2SemVer but include for optional use in C# script.
    ///     </para>
    /// </remarks>
    [Output]
    public string Version { get; set; } = "";

    /// <summary>
    ///     Optional MSBuild
    ///     <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#versionprefix">VersionPrefix</see>
    ///     property value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    /// </remarks>
    [Output]
    public string VersionPrefix { get; set; } = "";

    /// <summary>
    ///     Optional MSBuild
    ///     <see href="https://gist.github.com/jonlabelle/34993ee032c26420a0943b1c9d106cdc#versionsuffix">VersionSuffix</see>
    ///     property value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task input and output.
    ///     </para>
    ///     <para>
    ///         If set to <c>release</c> (case-insensitive) forces the version to be a release.
    ///         Otherwise, if set, forces a prerelease with this as the prerelease label.
    ///     </para>
    /// </remarks>
    /// 
    [Output]
    public string VersionSuffix { get; set; } = "";

    /// <summary>
    ///     Output that may be set by user's C# script.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    ///     <para>
    ///         Not used or set by Git2SemVer.
    ///         Made available to give C# script code an opportunity to output values to MSBuild.
    ///     </para>
    /// </remarks>
    [Output]
    public string Output1 { get; set; } = "";

    /// <summary>
    ///     Output that may be set by user's C# script.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MSBuild task output.
    ///     </para>
    ///     <para>
    ///         Not used or set by Git2SemVer.
    ///         Made available to give C# script code an opportunity to output values to MSBuild.
    ///     </para>
    /// </remarks>
    [Output]
    public string Output2 { get; set; } = "";

    protected void SetOutputs(IVersionOutputs outputs)
    {
        Version = outputs.Version?.ToString() ?? "";
        VersionSuffix = outputs.Version?.Prerelease ?? "";
        VersionPrefix = outputs.Version?.WithoutPrerelease().WithoutMetadata().ToString() ?? "";
        AssemblyVersion = outputs.AssemblyVersion?.ToString() ?? "";
        FileVersion = outputs.FileVersion?.ToString() ?? "";
        InformationalVersion = outputs.InformationalVersion?.ToString() ?? "";
        PackageVersion = outputs.PackageVersion?.ToString() ?? "";
        Output1 = outputs.Output1;
        Output2 = outputs.Output2;
        CommitsSinceLastRelease = outputs.Git.CommitsSinceLastRelease;
        IsRelease = outputs.Version?.IsRelease ?? false;

        LastReleaseCommitId = outputs.Git.LastReleaseCommit?.CommitId.Id ?? "";
        LastReleaseVersion = outputs.Git.LastReleaseVersion?.ToString() ?? "";
    }
}