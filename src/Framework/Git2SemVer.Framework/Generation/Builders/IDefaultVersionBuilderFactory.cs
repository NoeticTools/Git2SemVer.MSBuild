using Semver;


namespace NoeticTools.Git2SemVer.Framework.Generation.Builders;

internal interface IDefaultVersionBuilderFactory
{
    IVersionBuilder Create(SemVersion semanticVersion);
}