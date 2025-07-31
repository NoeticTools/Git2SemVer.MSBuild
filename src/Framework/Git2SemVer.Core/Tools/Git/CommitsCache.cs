using NoeticTools.Git2SemVer.Core.Exceptions;


namespace NoeticTools.Git2SemVer.Core.Tools.Git;

#pragma warning disable CS1591
public sealed class CommitsCache : ICommitsCache
{
    private readonly Dictionary<string, Commit> _commitsBySha = [];

    public void Add(params Commit[] commits)
    {
        foreach (var commit in commits)
        {
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryAdd
            if (!_commitsBySha.ContainsKey(commit.CommitId.Sha))
            {
                _commitsBySha.Add(commit.CommitId.Sha, commit);
            }
        }
    }

    public void Add(IReadOnlyList<Commit> commits)
    {
        Add(commits.ToArray());
    }

    public Commit Get(CommitId commitId)
    {
        return Get(commitId.Sha);
    }

    public Commit Get(string commitSha)
    {
        if (!TryGet(commitSha, out var commit))
        {
            throw new Git2SemVerRepositoryException($"Commit {commitSha} not found in the repository. Did you mean to use 'TryGet'?");
        }

        return commit;
    }

    public bool TryGet(CommitId commitId, out Commit commit)
    {
        return TryGet(commitId.Sha, out commit);
    }

    public bool TryGet(string commitSha, out Commit commit1)
    {
        return _commitsBySha.TryGetValue(commitSha, out commit1!);
    }
}