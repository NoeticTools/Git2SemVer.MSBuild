using NoeticTools.Common.Exceptions;
using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal abstract class BuildHostBase : ToolBase
{
    private readonly ILogger _logger;
    private string _buildContext = "UNKNOWN";
    private string _buildIdFormat = "";
    private string _buildNumber = "UNKNOWN";
    private Func<IReadOnlyList<string>> _buildNumberFunc;
    private Func<IReadOnlyList<string>> _defaultBuildNumberFunc;

    protected BuildHostBase(ILogger logger)
    {
        _logger = logger;
        _defaultBuildNumberFunc = () => [BuildNumber];
        _buildNumberFunc = _defaultBuildNumberFunc;
    }

    public string BuildContext
    {
        get => _buildContext;
        set
        {
            if (_buildContext.Equals(value, StringComparison.Ordinal))
            {
                return;
            }

            _buildContext = value;
            _logger.LogDebug("Build context set to {0}.", _buildContext);
        }
    }

    public IReadOnlyList<string> BuildId => _buildNumberFunc();

    public string BuildIdFormat
    {
        get => _buildIdFormat;
        set
        {
            if (_buildIdFormat == value)
            {
                return;
            }

            _buildIdFormat = value;
            _logger.LogDebug("Build ID format set to {0}.", _buildIdFormat);
            SetBuildIdFunc();
        }
    }

    public string BuildNumber
    {
        get => _buildNumber;
        set
        {
            if (_buildNumber.Equals(value, StringComparison.Ordinal))
            {
                return;
            }

            _buildNumber = value;
            _logger.LogDebug("Build number set to {0}.", _buildNumber);
        }
    }

    private string[] CustomBuildIdFormat()
    {
        if (string.IsNullOrWhiteSpace(BuildIdFormat))
        {
            throw new Git2SemVerConfigurationException("Host `BuildIdFormat` is required to generate a custom build ID.");
        }

        var buildId = BuildIdFormat.Replace("BUILD_NUMBER", BuildNumber)
                                   .Replace("BUILD_CONTEXT", BuildContext);
        return buildId.Split('.');
    }

    private void SetBuildIdFunc()
    {
        _buildNumberFunc = BuildIdFormat.Length > 0 ? CustomBuildIdFormat : DefaultBuildNumberFunc;
    }

    protected Func<IReadOnlyList<string>> DefaultBuildNumberFunc
    {
        get => _defaultBuildNumberFunc;
        set
        {
            _defaultBuildNumberFunc = value;
            SetBuildIdFunc();
        }
    }
}