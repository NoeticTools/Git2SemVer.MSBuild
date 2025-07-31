namespace NoeticTools.Git2SemVer.Core.Tools.Git;

public interface IGitTool : IDisposable
{
    /// <summary>
    ///     The current head's branch name.
    /// </summary>
    string BranchName { get; }

    ICommitsCache Cache { get; }

    /// <summary>
    ///     True if there are uncommited local changes.
    /// </summary>
    bool HasLocalChanges { get; }

    Commit Head { get; }

    Commit Get(CommitId commitId);
}