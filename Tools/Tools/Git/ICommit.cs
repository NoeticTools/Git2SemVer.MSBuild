using Semver;
using System.Text.Json.Serialization;


namespace NoeticTools.Common.Tools.Git;

[JsonDerivedType(typeof(Commit), typeDiscriminator: "Commit")]
public interface ICommit
{
    CommitId CommitId { get; }

    string Summary { get; }

    CommitId[] Parents { get; }

    SemVersion? ReleasedVersion { get; }

    string MessageBody { get; }

    string Refs { get; }
}