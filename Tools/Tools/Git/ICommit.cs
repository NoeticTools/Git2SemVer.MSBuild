using Semver;


namespace NoeticTools.Common.Tools.Git;

public interface ICommit
{
    CommitId CommitId { get; }

    string Message { get; }

    CommitId[] Parents { get; }

    SemVersion? ReleasedVersion { get; }
}