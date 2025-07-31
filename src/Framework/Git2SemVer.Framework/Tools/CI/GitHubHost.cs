using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

internal class GitHubHost : BuildHostBase, IBuildHost
{
    public GitHubHost(ILogger logger) : base(logger)
    {
        Name = "GitHub";
        DefaultBuildNumberFunc = () => [BuildNumber, BuildContext];
    }

    public HostTypeIds HostTypeId => HostTypeIds.GitHub;

    public void Dispose()
    {
    }
}