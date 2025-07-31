using LibGit2Sharp;


namespace NoeticTools.Git2SemVer.Core.Tools.Git;

public sealed class GitTag : IGitTag
{
    private readonly Tag _inner;

    public GitTag(Tag inner)
    {
        _inner = inner;
    }

    public string CanonicalName => _inner.CanonicalName;

    public string FriendlyName => _inner.FriendlyName;

    public bool IsAnnotated => _inner.IsAnnotated;
}