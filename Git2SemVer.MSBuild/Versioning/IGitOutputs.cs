using NoeticTools.Common.Tools.Git;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

public interface IGitOutputs
{
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
    string BranchName { get; }

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
    int CommitsSinceLastRelease { get; }

    /// <summary>
    ///     True if there are local changes since the last commit.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Not used by Git2SemVer. Provided for C# script use.
    ///     </para>
    /// </remarks>
    bool HasLocalChanges { get; }

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
    ICommit HeadCommit { get; }

    /// <summary>
    ///     The last release's commit. Null if no prior release found.
    /// </summary>
    ICommit? LastReleaseCommit { get; }

    /// <summary>
    ///     The last release's version. Null if no prior release.
    /// </summary>
    SemVersion? LastReleaseVersion { get; }
}