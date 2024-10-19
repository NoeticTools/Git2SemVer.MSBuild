using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;
using NoeticTools.MSBuild.Tasking;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;

/// <summary>
///     The C# script runner. These properties are exposed as global when running from Git2SemVer/
/// </summary>
public interface IVersioningContext
{
    /// <summary>
    ///     The build's host properties. Get build number here.
    /// </summary>
    IBuildHost Host { get; }

    /// <summary>
    ///     MSBuild input properties.
    /// </summary>
    IVersionGeneratorInputs Inputs { get; }

    /// <summary>
    ///     MSBuild logger.
    /// </summary>
    ILogger Logger { get; }

    /// <summary>
    ///     Harvested MSBuild global properties for script use.
    /// </summary>
    MSBuildGlobalProperties MsBuildGlobalProperties { get; }

    /// <summary>
    ///     Outputs that the C# script may use. Available to other MSBuild tasks as MSBuild properties.
    /// </summary>
    IVersionOutputs Outputs { get; }
}