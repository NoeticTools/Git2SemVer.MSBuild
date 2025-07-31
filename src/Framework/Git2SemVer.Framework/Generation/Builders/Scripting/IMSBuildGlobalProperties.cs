namespace NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;

#pragma warning disable CS1591
public interface IMSBuildGlobalProperties
{
    /// <summary>
    ///     Dictionary of all available parameters.
    /// </summary>
// ReSharper disable once MemberCanBePrivate.Global
    IReadOnlyDictionary<string, string> All { get; }

    bool BuildingInsideVisualStudio { get; }

    string Configuration { get; }

    string LangId { get; }

    string Language { get; }

    string Platform { get; }

    string ProjectDir { get; }

    string ProjectPath { get; }

    string SolutionDir { get; }

    string SolutionExt { get; }
}