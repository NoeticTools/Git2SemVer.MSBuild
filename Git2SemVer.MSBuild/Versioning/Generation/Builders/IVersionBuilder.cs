﻿using NoeticTools.Git2SemVer.Core.Tools.Git;
using NoeticTools.Git2SemVer.MSBuild.Framework.BuildHosting;


namespace NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders;

/// <summary>
///     An output builder that sets or updates any of the task's MSBuild output properties.
/// </summary>
public interface IVersionBuilder
{
    void Build(IBuildHost host, IGitTool gitTool, IVersionGeneratorInputs inputs, IVersionOutputs outputs);
}