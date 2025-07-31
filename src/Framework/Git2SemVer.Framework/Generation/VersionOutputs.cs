using System.Text.Json.Serialization;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;
using Semver;


namespace NoeticTools.Git2SemVer.Framework.Generation;

public sealed class VersionOutputs : IVersionOutputs
{
    [JsonConstructor]
    public VersionOutputs() : this(new GitOutputs(), new SemVersion(0, 0, 0))
    {
    }

    public VersionOutputs(GitOutputs gitOutputs, SemVersion version)
    {
        Git = gitOutputs;
        Version = version;
        InformationalVersion = version;
        PackageVersion = version;
        AssemblyVersion = version.ToVersion();
        FileVersion = AssemblyVersion;
    }

    [JsonPropertyOrder(25)]
    public Version? AssemblyVersion { get; set; }

    [JsonPropertyOrder(45)]
    public string BuildContext { get; set; } = "";

    [JsonPropertyOrder(35)]
    public string BuildNumber { get; set; } = "";

    [JsonPropertyOrder(20)]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? BuildSystemVersion { get; set; }

    [JsonPropertyOrder(30)]
    public Version? FileVersion { get; set; }

    [JsonPropertyOrder(70)]
    public IGitOutputs Git { get; }

    [JsonPropertyOrder(15)]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? InformationalVersion { get; set; }

    [JsonPropertyOrder(60)]
    public bool IsInInitialDevelopment { get; set; }

    [JsonIgnore]
    public bool IsValid => BuildNumber.Length > 0;

    [JsonPropertyOrder(90)]
    public string Output1 { get; set; } = "";

    [JsonPropertyOrder(91)]
    public string Output2 { get; set; } = "";

    [JsonPropertyOrder(17)]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? PackageVersion { get; set; }

    [JsonPropertyOrder(40)]
    public string PrereleaseLabel { get; set; } = "";

    [JsonPropertyOrder(10)]
    [JsonConverter(typeof(SemVersionJsonConverter))]
    public SemVersion? Version { get; set; }

    public string GetReport()
    {
        return $"""
                Outputs:
                   Assembly version:      {AssemblyVersion}
                   File version:          {FileVersion}
                   Package version:       {PackageVersion}
                   Build system label:    {BuildSystemVersion}
                   Informational version: {InformationalVersion}
                """;
    }

    public void SetAllVersionPropertiesFrom(SemVersion informationalVersion,
                                            string buildNumber,
                                            string buildContext)
    {
        Ensure.NotNull(informationalVersion, nameof(informationalVersion));

        SetAllVersionPropertiesFrom(informationalVersion);
        BuildNumber = buildNumber;
        BuildContext = buildContext;
    }

    public void SetAllVersionPropertiesFrom(SemVersion informationalVersion)
    {
        Ensure.NotNull(informationalVersion, nameof(informationalVersion));

        var version = informationalVersion.WithoutMetadata();
        var versionPrefix = informationalVersion.WithoutMetadata()
                                                .WithoutPrerelease();
        InformationalVersion = informationalVersion;
        Version = version;
        AssemblyVersion = new Version(versionPrefix.ToString());
        FileVersion = new Version(versionPrefix.ToString());
        PackageVersion = version;
        BuildSystemVersion = version;
        PrereleaseLabel = informationalVersion.IsRelease
            ? ""
            : informationalVersion.PrereleaseIdentifiers[0];
        IsInInitialDevelopment = informationalVersion.Major == 0;
    }
}