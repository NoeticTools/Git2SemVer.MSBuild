namespace NoeticTools.Git2SemVer.Framework.Generation.GitHistoryWalking;

internal interface IGitHistoryWalker
{
    SemanticVersionCalcResult CalculateSemanticVersion();
}