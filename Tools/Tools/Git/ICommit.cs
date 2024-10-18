using Semver;
using System.Text.Json.Serialization;


namespace NoeticTools.Common.Tools.Git;

[JsonDerivedType(typeof(Commit), typeDiscriminator: "Commit")]
public interface ICommit
{
    CommitId CommitId { get; }

    string Message { get; }

    CommitId[] Parents { get; }

    SemVersion? ReleasedVersion { get; }
}