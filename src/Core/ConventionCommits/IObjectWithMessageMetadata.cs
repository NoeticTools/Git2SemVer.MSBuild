namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface IObjectWithMessageMetadata
{
    ICommitMessageMetadata MessageMetadata { get; }
}