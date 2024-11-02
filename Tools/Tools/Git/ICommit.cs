using System.Text.Json.Serialization;
using Semver;


namespace NoeticTools.Common.Tools.Git;

[JsonDerivedType(typeof(Commit), "Commit")]
public interface ICommit
{
    CommitId CommitId { get; }

    string MessageBody { get; }

    CommitId[] Parents { get; }

    string Refs { get; }

    SemVersion? ReleasedVersion { get; }

    string Summary { get; }
}