using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal class GitHubHost : BuildHostBase, IBuildHost
{
    private readonly ILogger _logger;

    public GitHubHost(ILogger logger)
    {
        _logger = logger;
        BuildContext = "UNKNOWN";
        BuildNumber = "UNKNOWN";
        DefaultBuildNumberFunc = () => [BuildContext, BuildNumber];
    }

    public HostTypeIds HostTypeId => HostTypeIds.GitHub;

    public bool IsControlled => true;

    public string Name => "GitHub";

    public string BumpBuildNumber()
    {
        // Not supported
        return BuildNumber;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        // Not supported
    }

    public void ReportBuildStatistic(string key, double value)
    {
        // Not supported
    }

    public void SetBuildLabel(string label)
    {
        // Not supported
    }
}