namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

internal static class VersioningConstants
{
    public const string BranchMaturityPatternReleaseGroupName = "release";
    public const string DefaultBranchMaturityPattern = "^((?<release>main|release)|(?<Beta>feature)|(?<Alpha>.+))[\\/_]?";
    public const string DefaultInitialDevelopmentLabel = "InitialDev";
}