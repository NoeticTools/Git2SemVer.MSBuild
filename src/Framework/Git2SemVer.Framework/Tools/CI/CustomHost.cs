using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

internal class CustomHost : BuildHostBase, IDetectableBuildHost
{
    public CustomHost(ILogger logger) : base(logger)
    {
        Name = "Custom";
        BuildNumber = "UNKNOWN";
        BuildContext = "UNKNOWN";
        DefaultBuildNumberFunc = () => [BuildContext, BuildNumber];
    }

    public HostTypeIds HostTypeId => HostTypeIds.Custom;

    public void Dispose()
    {
    }

    public bool MatchesHostSignature()
    {
        // Not detectable
        return false;
    }
}