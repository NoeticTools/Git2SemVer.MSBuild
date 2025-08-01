using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;


namespace NoeticTools.Git2SemVer.Framework.Versioning.Builders;

/// <summary>
///     An output builder that sets or updates any of the task's MSBuild output properties.
/// </summary>
public interface IVersionBuilder
{
    void Build(IBuildHost host, IGitTool gitTool, IVersionGeneratorInputs inputs, IVersionOutputs outputs,
               IMSBuildGlobalProperties msBuildGlobalProperties);
}