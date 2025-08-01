using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Versioning.GitHistoryWalking;

internal interface IGitSegmentFactory
{
    GitSegment Create(params Commit[] commits);
}