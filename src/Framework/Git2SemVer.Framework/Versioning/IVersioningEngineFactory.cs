using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Framework.Framework.BuildHosting;
using NoeticTools.Git2SemVer.Framework.Persistence;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;


namespace NoeticTools.Git2SemVer.Framework.Versioning;

public interface IVersioningEngineFactory
{
    IVersioningEngine Create(IVersionGeneratorInputs inputs,
                             IMSBuildGlobalProperties msBuildGlobalProperties,
                             IOutputsJsonIO outputsJsonIO,
                             IBuildHost host,
                             ConventionalCommitsSettings convCommitsSettings);
}