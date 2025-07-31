namespace NoeticTools.Git2SemVer.Framework.Generation.Builders;

internal static class VersioningConstants
{
    public const string DefaultBranchMaturityPattern =
        "^((?<rc>(main|master|release)[\\\\\\/_](.*[\\\\\\/_])?rc.*)|(?<release>main|release)|(?<beta>feature)|(?<alpha>.+))[\\\\\\/_]?";

    public const string InitialDevelopmentLabel = "InitialDev";
    public const string ReleaseGroupName = "release";
}