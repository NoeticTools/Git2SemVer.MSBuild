using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.ChangeLogging;


public sealed class ConventionalCommit(Commit commit) : IChangeTypeAndDescription
{
    public string ChangeDescription { get; set; } = commit.MessageMetadata.ChangeDescription;

    public string ChangeType { get; set; } = commit.MessageMetadata.ChangeType;

    public IReadOnlyList<string> Issues { get; set; } = commit.MessageMetadata.Issues;

    public IDictionary<string, List<string>> FooterKeyValues { get; set; } = commit.MessageMetadata.FooterKeyValues;

    public string Sha { get; set; } = commit.CommitId.Sha;
}
