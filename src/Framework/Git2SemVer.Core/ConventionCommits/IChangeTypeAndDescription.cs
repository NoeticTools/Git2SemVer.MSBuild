using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface IChangeTypeAndDescription
{
    /// <summary>
    ///     The raw change type text found in the commit message.
    ///     Useful if the <c>ChangeType</c> is <c>CommitChangeTypeId.Custom</c>.
    /// </summary>
    [JsonPropertyOrder(22)]
    string ChangeType { get; }

    [JsonPropertyOrder(21)]

    string Description { get; }

    [JsonPropertyOrder(23)]
    string Scope { get; }
}