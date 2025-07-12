using System.Diagnostics;
using System.Text;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.ChangeLogging;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// ReSharper disable CanSimplifyDictionaryLookupWithTryAdd

namespace NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;

internal sealed class GitHistoryWalker(IGitTool gitTool, ILogger logger) : IGitHistoryWalker
{
    public SemanticVersionCalcResult CalculateSemanticVersion()
    {
        var stopwatch = Stopwatch.StartNew();
        var head = gitTool.Head;
        logger.LogDebug("Calculating semantic version for head '{0}'.", head.CommitId.ShortSha);

        SemanticVersionCalcResult result;

        using (logger.EnterLogScope())
        {
            var contributing = new GitSegmentsBuilder(gitTool, logger).GetContributingCommits(head);
            result = new GitSegmentsWalker(contributing, logger).CalculateSemVer();
        }

        stopwatch.Stop();
        logger.LogInfo("Calculated semantic version {0} from released ver {2} from commit '{1}' (in {3:F0} ms).",
                       result.Version,
                       result.PriorReleaseCommitId.ShortSha,
                       result.PriorReleaseVersion,
                       stopwatch.Elapsed.TotalMilliseconds);

        return result;
    }
}