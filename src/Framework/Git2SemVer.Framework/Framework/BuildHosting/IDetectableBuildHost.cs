namespace NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;

internal interface IDetectableBuildHost : IBuildHost
{
    bool MatchesHostSignature();
}