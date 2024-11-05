using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal class GitHubHost : BuildHostBase, IBuildHost
{
    private readonly ILogger _logger;

    public GitHubHost(ILogger logger) : base(logger)
    {
        _logger = logger;
        DefaultBuildNumberFunc = () => [BuildNumber, BuildContext];
    }

    public HostTypeIds HostTypeId => HostTypeIds.GitHub;

    public string Name => "GitHub";

    public string BumpBuildNumber()
    {
        // Not supported
        return BuildNumber;
    }

    public void ReportBuildStatistic(string key, int value)
    {
        _logger.LogTrace("GitHub host does not support build statistics.");
    }

    public void ReportBuildStatistic(string key, double value)
    {
        _logger.LogTrace("GitHub host does not support build statistics.");
    }

    public void SetBuildLabel(string label)
    {
        _logger.LogTrace("GitHub host does not support setting a build label.");
    }
}