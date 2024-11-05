using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.GitHistoryWalking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;

internal interface IDefaultVersionBuilderFactory
{
    IVersionBuilder Create(HistoryPaths historyPaths);
}