namespace NoeticTools.Git2SemVer.Framework.Versioning.Builders;

public static class VersioningConstants
{
    public const string DefaultBranchMaturityPattern =
        "^((?<rc>(main|master|release)[\\\\\\/_](.*[\\\\\\/_])?rc.*)|(?<release>main|release)|(?<beta>feature)|(?<alpha>.+))[\\\\\\/_]?";

    public const string DefaultScriptFilename = "Git2SemVer.csx";

    public const string InitialDevelopmentLabel = "InitialDev";
    public const string ReleaseGroupName = "release";

    public const string SharedVersionJsonPropertiesFilename = "Git2SemVer.VersionInfo.g.json";
}