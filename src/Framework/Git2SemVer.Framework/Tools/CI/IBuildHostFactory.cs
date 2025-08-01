using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

public interface IBuildHostFactory
{
    IBuildHost Create(string hostType, string buildNumber, string buildContext, string inputsBuildIdFormat);
}