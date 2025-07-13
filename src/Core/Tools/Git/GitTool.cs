using LibGit2Sharp;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Exceptions;
using NoeticTools.Git2SemVer.Core.Tools.Git.Parsers;


#pragma warning disable CS1591

namespace NoeticTools.Git2SemVer.Core.Tools.Git;

[RegisterTransient]
public sealed class GitTool : IGitTool
{
    private const int TakeLimit = 300;
    private readonly ICommitsCache? _cache;
    private readonly IConventionalCommitsParser _metadataParser;
    private readonly ITagParser _tagParser;
    private Commit? _head;
    private bool _initialised;
    private Repository? _repository;
    private string _repositoryDirectory = null!;

    public GitTool(ITagParser tagParser, IConventionalCommitsParser convCommitsParser)
    {
        _tagParser = tagParser;
        _cache = new CommitsCache();
        if (string.IsNullOrEmpty(RepositoryDirectory))
        {
            RepositoryDirectory = Environment.CurrentDirectory;
        }
        _metadataParser = convCommitsParser;
    }

    public string BranchName => Repository.Head.FriendlyName;

    public ICommitsCache Cache
    {
        get
        {
            if (_cache == null)
            {
                PrimeCache();
            }

            return _cache!;
        }
    }

    public bool HasLocalChanges => GetHasLocalChanges();

    public Commit Head
    {
        get
        {
            if (_head == null)
            {
                PrimeCache();
            }

            return _head!;
        }
    }

    public string RepositoryDirectory
    {
        get => _repositoryDirectory;
        set => _repositoryDirectory = DiscoverRepositoryDirectory(value);
    }

    public void Dispose()
    {
        _repository?.Dispose();
    }

    public Commit Get(CommitId commitId)
    {
        var commitSha = commitId.Sha;

        if (Cache.TryGet(commitSha, out var existingCommit))
        {
            return existingCommit;
        }

        var commits = GetReachableFrom(commitSha);
        if (commits.Count == 0)
        {
            throw new Git2SemVerRepositoryException($"Unable to find git commit '{commitSha}' in the repository.");
        }

        Cache.Add(commits);
        return Cache.Get(commitSha);
    }

    private Repository Repository => _repository ??= new Repository(RepositoryDirectory);

    private Commit Convert(LibGit2Sharp.Commit rawCommit)
    {
        if (!Cache.TryGet(rawCommit.Sha, out var commit))
        {
            var parents = rawCommit.Parents.Select(x => x.Sha).ToArray();
            var metadata = _metadataParser.Parse(rawCommit.MessageShort, rawCommit.Message);
            var tags = Repository.Tags.Where(x => x.Target.Equals(rawCommit)).Select(x => new GitTag(x)).ToList();

            commit = new Commit(rawCommit.Sha,
                                parents, rawCommit.MessageShort,
                                metadata, _tagParser,
                                tags,
                                rawCommit.Author.When);

            Cache.Add(commit);
        }

        return commit;
    }

    private static string DiscoverRepositoryDirectory(string currentDirectory)
    {
        if (currentDirectory.EndsWith(".git"))
        {
            return currentDirectory;
        }

        var discoveredDirectory = Repository.Discover(currentDirectory);
        if (discoveredDirectory == null)
        {
            throw new Git2SemVerRepositoryException($"Unable to find git repository from directory '{currentDirectory}'.");
        }

        return new DirectoryInfo(discoveredDirectory).Parent!.FullName;
    }

    private bool GetHasLocalChanges()
    {
        return Task.Run(GetHasLocalChangesAsync).Result;
    }

    private Task<bool> GetHasLocalChangesAsync()
    {
        var statusOptions = new StatusOptions();
        var status = Repository.RetrieveStatus(statusOptions);
        return Task.FromResult(status.IsDirty);
    }

    private void PrimeCache()
    {
        if (_initialised)
        {
            throw new InvalidOperationException($"The method commit tool's {nameof(PrimeCache)} is called more than once.");
        }

        _initialised = true;

        var commits = Repository.Commits.Take(TakeLimit).Select(Convert).ToList();
        if (!commits.Any())
        {
            throw new Git2SemVerGitOperationException("Unable to get commits. Either new repository and no commits or problem accessing git.");
        }

        _head = commits[0];
    }

    internal IReadOnlyList<Commit> GetReachableFrom(string commitSha)
    {
        return Repository.Commits.QueryBy(new CommitFilter
        {
            IncludeReachableFrom = commitSha
        }).Take(TakeLimit).Select(Convert).ToList();
    }
}