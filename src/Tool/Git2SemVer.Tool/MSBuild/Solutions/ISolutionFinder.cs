namespace NoeticTools.Git2SemVer.Tool.MSBuild.Solutions;

internal interface ISolutionFinder
{
    FileInfo Find(string inputSolutionFile);
}