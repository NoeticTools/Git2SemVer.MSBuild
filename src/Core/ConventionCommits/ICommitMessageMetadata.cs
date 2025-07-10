using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface ICommitMessageMetadata : IChangeTypeAndDescription
{
    [JsonPropertyOrder(11)]
    ApiChangeFlags ApiChangeFlags { get; }

    [JsonPropertyOrder(12)]
    string Body { get; }

    [JsonPropertyOrder(40)]
    Dictionary<string, List<string>> FooterKeyValues { get; }

    [JsonIgnore]
    IReadOnlyList<string> Issues { get; }
}