using System.Globalization;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Framework.Config;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;


namespace NoeticTools.Git2SemVer.Framework.Tools.CI;

[RegisterTransient]
internal sealed class UncontrolledHost : BuildHostBase, IDetectableBuildHost
{
    private readonly ILocalSettings _config;
    private readonly ILogger _logger;

    public UncontrolledHost(ILocalSettings config, ILogger logger) : base(logger)
    {
        _config = config;
        _logger = logger;
        Name = "Uncontrolled";
        BuildContext = Environment.MachineName.ToNormalisedSemVerIdentifier();
        BuildNumber = _config.BuildNumber.ToString(CultureInfo.InvariantCulture);
        DefaultBuildNumberFunc = () => [BuildContext, BuildNumber];
    }

    public HostTypeIds HostTypeId => HostTypeIds.Uncontrolled;

    public override string BumpBuildNumber()
    {
        _config.BuildNumber++;
        _config.Save();
        _logger.LogDebug("Build number bumped to {0}.", _config.BuildNumber);
        BuildNumber = _config.BuildNumber.ToString(CultureInfo.InvariantCulture);
        return BuildNumber;
    }

    public void Dispose()
    {
    }

    public bool MatchesHostSignature()
    {
        return true;
    }
}