using NoeticTools.Common.Exceptions;
using NoeticTools.Git2SemVer.MSBuild.Framework;


namespace NoeticTools.Git2SemVer.MSBuild.Tools.CI;

internal abstract class BuildHostBase : ToolBase
{
    private string _buildIdFormat = "";
    private Func<IReadOnlyList<string>> _buildNumberFunc;
    private Func<IReadOnlyList<string>> _defaultBuildNumberFunc;

    protected BuildHostBase()
    {
        _defaultBuildNumberFunc = () => [$"{BuildNumber}"];
        _buildNumberFunc = _defaultBuildNumberFunc;
    }

    public string BuildContext { get; set; } = "UNKNOWN";

    public IReadOnlyList<string> BuildId => _buildNumberFunc();

    public string BuildIdFormat
    {
        get => _buildIdFormat;
        set
        {
            if (_buildIdFormat != value)
            {
                _buildIdFormat = value;
                SetBuildIdFunc();
            }
        }
    }

    public string BuildNumber { get; set; } = "UNKNOWN";

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

    private IReadOnlyList<string> CustomBuildIdFormat()
    {
        if (string.IsNullOrWhiteSpace(BuildIdFormat))
        {
            throw new Git2SemVerConfigurationException("Host `BuildIdFormat` is required to generate a custom build ID.");
        }

        var buildId = BuildIdFormat.Replace("BUILD_NUMBER", BuildNumber)
                                   .Replace("BUILD_CONTEXT", BuildContext);
        return buildId.Split('.');
    }
}