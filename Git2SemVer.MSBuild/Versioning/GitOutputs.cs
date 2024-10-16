using System.Text.Json.Serialization;
using NoeticTools.Common.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

public sealed class GitOutputs
{
    [JsonConstructor]
    internal GitOutputs()
    {
    }

    internal GitOutputs(IGitTool gitTool, HistoryPaths paths)
    {
        LastReleaseVersion = paths.BestPath.LastReleasedVersion;
        LastReleaseCommit = paths.BestPath.LastReleasedVersion == null ? null : paths.BestPath.FirstCommit;
        HeadCommit = paths.HeadCommit;
        BranchName = gitTool.BranchName;
        CommitsSinceLastRelease = paths.BestPath.CommitsSinceLastRelease;
        HasLocalChanges = gitTool.HasLocalChanges;
    }

    /// <summary>
    ///     The local Git repository head's last commit SHA.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Not output from the MSBuild task.
    ///         Following tasks should use the MSBuild property
    ///         <see href="https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets#pack-target">RepositoryBranch</see>
    ///     </para>
    /// </remarks>
    public string BranchName { get; } = "";

    /// <summary>
    ///     The commit count (height) from the last release's commit to the head's last commit.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Not used by Git2SemVer. Provided for C# script use.
    ///     </para>
    ///     <para>
    ///         Using commit height is a popular practice in leu of a build number.
    ///         It is not always unique, does not increment on each build, and may not be reproducible.
    ///         Consider using the
    ///         <see cref="IBuildHost.BuildNumber">build host's BuildNumber</see> instead.
    ///     </para>
    /// </remarks>
    public int CommitsSinceLastRelease { get; }

    /// <summary>
    ///     True if there are local changes since the last commit.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Not used by Git2SemVer. Provided for C# script use.
    ///     </para>
    /// </remarks>
    public bool HasLocalChanges { get; }

    /// <summary>
    ///     The local Git repository head's last commit SHA.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Not output from the Git2SemVer MSBuild task.
    ///         Following MSBuild tasks should use the MSBuild property
    ///         <see href="https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets#pack-target">RepositoryCommit</see>
    ///     </para>
    /// </remarks>
    public Commit HeadCommit { get; } = Commit.Null;

    /// <summary>
    ///     The last release's commit. Null if no prior release found.
    /// </summary>
    public Commit? LastReleaseCommit { get; }

    /// <summary>
    ///     The last release's version. Null if no prior release.
    /// </summary>
    public SemVersion? LastReleaseVersion { get; }
}