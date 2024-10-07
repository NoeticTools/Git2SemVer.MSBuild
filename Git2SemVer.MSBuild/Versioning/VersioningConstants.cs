namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

internal static class VersioningConstants
{
    public const string BranchMaturityPatternReleaseGroupName = "release";
    public const string DefaultBranchMaturityPattern = "^((?<release>main|release)|(?<beta>feature)|(?<alpha>.+))[\\/_]?";
    public const string InitialDevelopmentLabel = "InitialDev";
}