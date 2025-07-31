namespace NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;

/// <summary>
///     MSBuild properties used when running outside a MSBuild build.
/// </summary>
#pragma warning disable CS1591
public class NullMSBuildGlobalProperties : IMSBuildGlobalProperties
{
    public IReadOnlyDictionary<string, string> All { get; } = new Dictionary<string, string>();

    public bool BuildingInsideVisualStudio { get; } = false;

    public string Configuration { get; } = string.Empty;

    public string LangId { get; } = string.Empty;

    public string Language { get; } = string.Empty;

    public string Platform { get; } = string.Empty;

    public string ProjectDir { get; } = string.Empty;

    public string ProjectPath { get; } = string.Empty;

    public string SolutionDir { get; } = string.Empty;

    public string SolutionExt { get; } = string.Empty;
}