using System.Text.Json.Serialization;


namespace NoeticTools.Git2SemVer.Core.ConventionCommits;

public interface IChangeMessageMetadata : IChangeTypeAndDescription
{
    [JsonIgnore]
    IReadOnlyList<string> Issues { get; }
}