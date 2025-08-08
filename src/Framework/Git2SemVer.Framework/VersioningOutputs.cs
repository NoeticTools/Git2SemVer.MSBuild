using NoeticTools.Git2SemVer.Framework.Versioning;


namespace NoeticTools.Git2SemVer.Framework;

public class VersioningOutputs(IVersionOutputs version, SemanticVersionCalcResult? metadata)
{
    public SemanticVersionCalcResult Metadata { get; } = metadata ?? new SemanticVersionCalcResult();

    public IVersionOutputs Versions { get; } = version;
}