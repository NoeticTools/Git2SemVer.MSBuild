using NoeticTools.Common.Tools.Git;
using Semver;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;

#pragma warning disable CS1591
internal interface IVersionHistoryPath
{
    int CommitsSinceLastRelease { get; }

    Commit FirstCommit { get; }

    Commit HeadCommit { get; }

    int Id { get; }

    SemVersion? LastReleasedVersion { get; }

    SemVersion Version { get; }

    string ToString();
    IReadOnlyList<VersionHistoryPath> With(IReadOnlyList<VersionHistorySegment> toSegments);
}