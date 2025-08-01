using Semver;


namespace NoeticTools.Git2SemVer.Framework.Versioning.Builders;

internal interface IDefaultVersionBuilderFactory
{
    IVersionBuilder Create(SemVersion semanticVersion);
}