using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface ICommitMessageMetadata : IChangeMessageMetadata
{
    [JsonPropertyOrder(1)]
    ApiChangeFlags ApiChangeFlags { get; }

    [JsonPropertyOrder(2)]
    string Body { get; }

    [JsonPropertyOrder(6)]
    ILookup<string, string> FooterKeyValues { get; }
}