using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;


// ReSharper disable UnusedMemberInSuper.Global

namespace NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;

/// <summary>
///     The C# script runner. These properties are exposed as global when running from Git2SemVer/
/// </summary>
public interface IVersioningContext
{
    /// <summary>
    ///     Git tool for running git commands.
    /// </summary>
    IGitTool Git { get; }

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

    ///// <summary>
    /////     Harvested MSBuild global properties for script use.
    ///// </summary>
    IMSBuildGlobalProperties MsBuildGlobalProperties { get; }

    /// <summary>
    ///     Outputs that the C# script may use. Available to other MSBuild tasks as MSBuild properties.
    /// </summary>
    IVersionOutputs Outputs { get; }
}