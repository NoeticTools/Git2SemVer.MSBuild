namespace NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;

internal interface IGitHistoryWalker
{
    SemanticVersionCalcResult CalculateSemanticVersion();
}