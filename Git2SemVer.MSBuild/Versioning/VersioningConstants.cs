namespace NoeticTools.Git2SemVer.MSBuild.Versioning;

internal static class VersioningConstants
{
    public const string ReleaseGroupName = "release";
    public const string DefaultBranchMaturityPattern = "^((?<release>main|release)|(?<Beta>feature)|(?<Alpha>.+))[\\/_]?";
    public const string InitialDevelopmentLabel = "InitialDev";
}