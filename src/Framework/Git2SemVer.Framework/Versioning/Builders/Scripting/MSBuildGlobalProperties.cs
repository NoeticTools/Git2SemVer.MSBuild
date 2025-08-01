using System.Text.RegularExpressions;
using Microsoft.Build.Framework;


#pragma warning disable CA1507

// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;

/// <summary>
///     [MSBuild current project global
///     properties](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.ibuildengine6.getglobalproperties?view=msbuild-17-netcore)
///     for optional C# script use.
/// </summary>
public sealed class MSBuildGlobalProperties : IMSBuildGlobalProperties
{
    /// <summary>
    ///     Constructor to create wrapper for [MSBuild current project global
    ///     properties](https://learn.microsoft.com/en-us/dotnet/api/microsoft.build.framework.ibuildengine6.getglobalproperties?view=msbuild-17-netcore)
    ///     read from the build engine.
    /// </summary>
    public MSBuildGlobalProperties(IBuildEngine6 buildEngine)
    {
        All = buildEngine.GetGlobalProperties();
        Language = GetStringValue("Language");
        Configuration = GetStringValue("Configuration");
        LangId = GetStringValue("LangId");
        SolutionDir = GetStringValue("SolutionDir");
        Platform = GetStringValue("Platform");
        SolutionExt = GetStringValue("SolutionExt");
        BuildingInsideVisualStudio = GetBoolValue("BuildingInsideVisualStudio");

        var projectConfiguration = GetStringValue("ProjectConfiguration");
        if (string.IsNullOrWhiteSpace(projectConfiguration))
        {
            return;
        }

        var match = Regex.Match(projectConfiguration, "AbsolutePath=\"(?<path>.*)\">");
        if (!match.Success)
        {
            return;
        }

        var projectPath = match.Groups["path"].Value;
        ProjectDir = Path.GetDirectoryName(projectPath)!;
        ProjectPath = projectPath;
    }

    /// <summary>
    ///     Dictionary of all available parameters.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public IReadOnlyDictionary<string, string> All { get; }

    public bool BuildingInsideVisualStudio { get; }

    public string Configuration { get; }

    public string LangId { get; }

    public string Language { get; }

    public string Platform { get; }

    public string ProjectDir { get; } = "";

    public string ProjectPath { get; } = "";

    public string SolutionDir { get; }

    public string SolutionExt { get; }

    private bool GetBoolValue(string propertyName)
    {
        return All.TryGetValue(propertyName, out var value) && bool.Parse(value);
    }

    private string GetStringValue(string propertyName)
    {
        return All.TryGetValue(propertyName, out var value) ? value : string.Empty;
    }
}