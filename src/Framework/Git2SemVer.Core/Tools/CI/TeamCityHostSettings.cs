using System.Globalization;


// ReSharper disable MemberCanBeMadeStatic.Global

namespace NoeticTools.Git2SemVer.Core.Tools.CI;

public static class TeamCityHostSettings
{
    private const string BuildNumberEnvVarName = "BUILD_NUMBER";
    private const string TeamCityVersionEnvVarName = "TEAMCITY_VERSION";

    public static string BuildNumber
    {
        get
        {
            var buildNumberVariable = Environment.GetEnvironmentVariable(BuildNumberEnvVarName);
            if (buildNumberVariable == null)
            {
                return "";
            }

            return int.TryParse(buildNumberVariable, out var buildNumber) ? buildNumber.ToString(CultureInfo.InvariantCulture) : "";
        }
    }

    public static string Version => Environment.GetEnvironmentVariable(TeamCityVersionEnvVarName) ?? "";

    /// <summary>
    ///     Returns true currently hosted on a teamcity agent.
    /// </summary>
    public static bool IsHost()
    {
        return !string.IsNullOrWhiteSpace(Version) &&
               !string.IsNullOrWhiteSpace(BuildNumber);
    }
}