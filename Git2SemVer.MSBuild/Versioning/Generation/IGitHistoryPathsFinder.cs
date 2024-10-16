using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;

internal interface IGitHistoryPathsFinder
{
    HistoryPaths FindPathsToHead();
}