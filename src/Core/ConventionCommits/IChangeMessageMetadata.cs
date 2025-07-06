namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface IChangeMessageMetadata
{
    string ChangeDescription { get; }

    /// <summary>
    ///     The raw change type text found in the commit message.
    ///     Useful if the <c>ChangeType</c> is <c>CommitChangeTypeId.Custom</c>.
    /// </summary>
    string ChangeTypeText { get; }

    IReadOnlyList<string> Issues { get; }
}