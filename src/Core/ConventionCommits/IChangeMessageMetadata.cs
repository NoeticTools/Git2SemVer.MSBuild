using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface IChangeMessageMetadata
{
    [JsonPropertyOrder(2)]
    string ChangeDescription { get; }

    [JsonPropertyOrder(4)]
    CommitChangeTypeId ChangeType { get; }

    /// <summary>
    ///     The raw change type text found in the commit message.
    ///     Useful if the <c>ChangeType</c> is <c>CommitChangeTypeId.Custom</c>.
    /// </summary>
    [JsonPropertyOrder(5)]
    string ChangeTypeText { get; }

    [JsonIgnore]
    IReadOnlyList<string> Issues { get; }
}