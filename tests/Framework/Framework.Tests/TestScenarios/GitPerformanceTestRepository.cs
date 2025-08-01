using NoeticTools.Git2SemVer.Core.ConventionCommits;
using NoeticTools.Git2SemVer.Core.Tools.Git;


namespace NoeticTools.Git2SemVer.Framework.Tests.TestScenarios;

public class GitPerformanceTestRepository : GitTestRepository
{
    public GitPerformanceTestRepository(string description)
        : base(description, [], "000.01.000", 0, "0.1.0")
    {
        var commits = new List<Commit>
        {
            NewCommit("00.0.01.000", [], "First commit in repo")
        };

        var endOfPriorBlockCommitId = "00.0.01.000";
        var headCommitId = endOfPriorBlockCommitId;

        for (var blockNumber = 1; blockNumber <= 12; blockNumber++)
        {
            var branchPrefix = blockNumber.ToString("D2");

            commits.AddRange(
            [
                // left (main) branch
                NewCommit(branchPrefix + ".1.01.000", [endOfPriorBlockCommitId], $"bottom (oldest) end of block {blockNumber}"),
                NewCommit(branchPrefix + ".1.02.000", [branchPrefix + ".1.01.000"], "Branch from"),
                NewCommit(branchPrefix + ".1.03.000", [branchPrefix + ".1.02.000"], ""),
                NewCommit(branchPrefix + ".1.04.000", [branchPrefix + ".1.03.000", branchPrefix + ".2.03.000"], "Merge"),
                NewCommit(branchPrefix + ".1.05.000", [branchPrefix + ".1.04.000", branchPrefix + ".3.03.000"], "Merge"),
                NewCommit(branchPrefix + ".1.06.000", [branchPrefix + ".1.05.000"], $"top (newest) of block {blockNumber}"),

                // branch 2 (middle)
                NewCommit(branchPrefix + ".2.01.000", [branchPrefix + ".1.02.000"], "Branch"),
                NewCommit(branchPrefix + ".2.02.000", [branchPrefix + ".2.01.000"], ""),
                NewCommit(branchPrefix + ".2.03.000", [branchPrefix + ".2.02.000"], ""),

                // branch 3 (right)
                NewCommit(branchPrefix + ".3.01.000", [branchPrefix + ".1.02.000"], "Branch"),
                NewCommit(branchPrefix + ".3.02.000", [branchPrefix + ".3.01.000"], ""),
                NewCommit(branchPrefix + ".3.03.000", [branchPrefix + ".3.02.000"], "")
            ]);

            headCommitId = branchPrefix + ".1.06.000";
            endOfPriorBlockCommitId = headCommitId;
        }

        Commits = commits.ToArray();
        HeadCommitId = headCommitId;
    }

    private static Commit NewCommit(string sha, string[] parents, string summary, string? refs = "", ICommitMessageMetadata? metadata = null)
    {
        return new Commit(sha, parents, summary, refs, metadata ?? CommitMessageMetadata.Null);
    }
}