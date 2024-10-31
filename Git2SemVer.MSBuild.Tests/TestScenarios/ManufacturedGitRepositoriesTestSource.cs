using System.Collections;
using NoeticTools.Common.Tools.Git;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.TestScenarios;

public sealed class ManufacturedGitRepositoriesTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] { "Scenario 01", Scenario01 };
        yield return new object[] { "Scenario 02", Scenario02 };
        yield return new object[] { "Scenario 03", Scenario03 };
        yield return new object[] { "Scenario 04", Scenario04 };
        yield return new object[] { "Scenario 05", Scenario05 };
        yield return new object[] { "Scenario 06", Scenario06 };
    }

    private GitTestRepository Scenario01 { get; } =
        new("""
            Scenario 01:
              - Head is master branch (1)
              - No commit bump messages
              - No releases on master
              - 2 releases on a merged branch
              
              | head
              |
              |\____   branch 3
              |     \
              |      | v1.2.4
              |      | v1.2.3
              | ____/
              |/
              |
              |\____   branch 2
              |     \
              |      |
              | ____/
              |/
              |
              | first commit
            """,
            new[]
            {
                new Commit("1.001.0000", [], "First commit in repo", "", ""),
                new Commit("1.002.0000", ["1.001.0000"], "", "", "tag: A tag"),
                new Commit("1.003.0000", ["1.002.0000"], "", "", "tag: A6.7.8"),
                new Commit("1.004.0000", ["1.003.0000", "2.003.0000"], "Merge", "", ""),
                new Commit("1.005.0000", ["1.004.0000"], "", "", ""),
                new Commit("1.006.0000", ["1.005.0000"], "", "", ""),
                new Commit("1.007.0000", ["1.006.0000"], "", "", ""),
                new Commit("1.008.0000", ["1.007.0000", "3.005.0000"], "Merge", "", ""),
                new Commit("1.009.0000", ["1.008.0000"], "", "", ""),
                new Commit("1.010.0000", ["1.009.0000"], "Head commit", "", ""),

                new Commit("2.001.0000", ["1.001.0000"], "Branch commit", "", ""),
                new Commit("2.002.0000", ["2.001.0000"], "", "", ""),
                new Commit("2.003.0000", ["2.002.0000"], "", "", ""),

                new Commit("3.001.0000", ["1.007.0000"], "Branch commit", "", ""),
                new Commit("3.002.0000", ["3.001.0000"], "", "", "tag: v1.2.3"),
                new Commit("3.003.0000", ["3.002.0000"], "", "", "tag: v1.2.4"),
                new Commit("3.004.0000", ["3.003.0000"], "", "", ""),
                new Commit("3.005.0000", ["3.004.0000"], "", "", "")
            },
            "master",
            "1.010.0000",
            3,
            "1.2.5");

    private GitTestRepository Scenario02 { get; } =
        new("""
            Scenario 02:
              - Release on merge commit with release on branch being merged
              - Head is master branch (1)
              - No commit bump messages
              - Release 1.2.2 on master on branch merge commit
              - Release 1.2.4 on branch that has been merged to master

            1.010  | head (1)
            .      |
            .      |\____   branch 2 (merge) - v1.2.2
            .      |     \
            .      |      | v1.2.4
            .      | ____/
            .      |/
            .      |
            1.001  | first commit
            """,
            new[]
            {
                new Commit("1.001.0000", [], "First commit in repo", "", ""),
                new Commit("1.007.0000", ["1.001.0000"], "", "", "Branched from"),
                new Commit("1.008.0000", ["1.007.0000", "2.005.0000"], "Merge commit", "", "tag: v1.2.2"),
                new Commit("1.009.0000", ["1.008.0000"], "", "", ""),
                new Commit("1.010.0000", ["1.009.0000"], "Head commit", "", ""),

                new Commit("2.001.0000", ["1.007.0000"], "Branch commit", "", ""),
                new Commit("2.003.0000", ["2.001.0000"], "", "", "tag: v1.2.4"),
                new Commit("2.005.0000", ["2.003.0000"], "", "", "")
            },
            "master",
            "1.010.0000",
            1,
            "1.2.3");

    private GitTestRepository Scenario03 { get; } =
        new("""
            Scenario 03:
              - Single branch, takes first release
              - Head is master branch (1)
              - No commit bump messages
              - Release 1.5.9 on master
              - Release 2.2.2 on prior commit on master

            1.006  | head
            .      | v1.5.9
            .      |
            .      | v2.2.2
            .      |
            1.001  | first commit
            """,
            new[]
            {
                new Commit("1.001", [], "First commit in repo", "", ""),
                new Commit("1.002", ["1.001"], "", "", ""),
                new Commit("1.003", ["1.002"], "", "", "tag: v2.2.2"),
                new Commit("1.004", ["1.003"], "", "", ""),
                new Commit("1.005", ["1.004"], "", "", "tag: V1.5.9"),
                new Commit("1.006", ["1.005"], "", "", "Head commit")
            },
            "master",
            "1.006",
            1,
            "1.5.10");

    private GitTestRepository Scenario04 { get; } =
        new("""
            Scenario 04:
              - Single commit repository
              - No releases

            1.001  | head
            """,
            new[]
            {
                new Commit("1.001.0000", [], "First commit in repo", "", "")
            },
            "master",
            "1.001.0000",
            1,
            "0.1.0");

    private GitTestRepository Scenario05 { get; } =
        new("""
            Scenario 05:
              - Single commit repository
              - 1.0.0 released

            1.001  | head   v1.0.0
            """,
            new[]
            {
                new Commit("1.001.0000", [], "First commit in repo", "", "tag: v1.0.0")
            },
            "master",
            "1.001.0000",
            1,
            "1.0.1");

    private GitTestRepository Scenario06 { get; } =
        new("""
            Scenario 06:
              - Multiple parallel branches
              - Releases in every branch

            1.006  | head
            .      |
            1.005  |\____________________________   branch 3 (merge)
            1.004  |\_______   branch 2 (merge)   \
            .      |         \                     | v5.6.99  
            .      | v5.7.0   | v5.7.1             |
            .      | ________/____________________/
            .      |/
            .      |
            1.001  | first commit
            """,
            new[]
            {
                new Commit("1.001.0000", [], "First commit in repo", "", ""),
                new Commit("1.002.0000", ["1.001.0000"], "Branch from", "", ""),
                new Commit("1.003.0000", ["1.002.0000"], "", "", "tag: v5.7.0"),
                new Commit("1.004.0000", ["1.003.0000", "2.003.0000"], "Merge", "", ""),
                new Commit("1.005.0000", ["1.004.0000", "3.003.0000"], "Merge", "", ""),
                new Commit("1.006.0000", ["1.005.0000"], "Head commit", "", ""),

                new Commit("2.001.0000", ["1.002.0000"], "Branch", "", ""),
                new Commit("2.002.0000", ["2.001.0000"], "", "", "tag: v5.7.1"),
                new Commit("2.003.0000", ["2.002.0000"], "", "", ""),

                new Commit("3.001.0000", ["1.002.0000"], "Branch", "", ""),
                new Commit("3.002.0000", ["3.001.0000"], "", "", "tag: v5.6.99"),
                new Commit("3.003.0000", ["3.002.0000"], "", "", "")
            },
            "master",
            "1.006.0000",
            3,
            "5.7.2");
}