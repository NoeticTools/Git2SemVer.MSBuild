using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using Semver;


namespace NoeticTools.Git2SemVer.Framework;

[JsonDerivedType(typeof(GitOutputs), "GitOutputs")]
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
    ICommit? PriorReleaseCommit { get; }

    /// <summary>
    ///     The last release's version. Null if no prior release.
    /// </summary>
    SemVersion? PriorReleaseVersion { get; }

    /// <summary>
    ///     Prior releases that are directly reachable from the head commit. 
    /// </summary>
    /// <remarks>
    /// <p>
    ///     A git tree showing 2 prior contributing releases (1.2.4 & 1.3.0):
    /// </p>
    /// <code>
    ///       1.2.3        1.3.0          head
    ///   ------o------o-----o----o---o--- o
    ///          \                   /
    ///           o-----o-----------o
    ///               1.2.4
    /// </code>
    /// </remarks>
    SemVersion[] ContributingReleases { get; }
}