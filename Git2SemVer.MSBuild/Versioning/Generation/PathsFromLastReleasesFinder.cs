using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// ReSharper disable CanSimplifyDictionaryLookupWithTryAdd

namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

internal sealed class PathsFromLastReleasesFinder(ICommitsRepository commitsRepo, IGitTool gitTool, ILogger logger) : IGitHistoryPathsFinder
{
    public HistoryPaths FindPathsToHead()
    {
        //VersionHistorySegment.Reset();
        //CommitObfuscator.Clear();

        logger.LogDebug($"Current branch: {gitTool.BranchName}");
        var segments = new VersionHistorySegmentsBuilder(commitsRepo, logger).BuildTo(gitTool.Head);
        return new VersionHistoryPathsBuilder(segments, logger).Build();
    }
}