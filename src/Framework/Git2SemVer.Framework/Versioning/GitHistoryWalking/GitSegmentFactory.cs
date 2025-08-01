using System.Diagnostics.CodeAnalysis;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;

#pragma warning disable CS1591
[ExcludeFromCodeCoverage]
internal sealed class GitSegmentFactory(ILogger logger) : IGitSegmentFactory
{
    private int _nextId = 1;

    public GitSegment Create(params Commit[] commits)
    {
        return new GitSegment(_nextId++, commits, logger);
    }
}