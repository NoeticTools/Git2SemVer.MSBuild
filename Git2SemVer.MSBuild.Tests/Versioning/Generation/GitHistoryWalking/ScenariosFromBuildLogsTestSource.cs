using System.Collections;


namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.GitHistoryWalking;

internal sealed class ScenariosFromBuildLogsTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] { "Scenario 01", Scenario01 };
        yield return new object[] { "Scenario 02", Scenario02 };
        yield return new object[] { "Scenario 03", Scenario03 };
        yield return new object[] { "Scenario 04", Scenario04 };
        yield return new object[] { "Scenario 05", Scenario05 };
    }

    public LoggedScenario Scenario01 { get; } =
        new("0.3.2", "0001", """
                            *               .|0001|0007 0002|REDACTED| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                            |\          
                            | *             .|0002|0003|REDACTED||
                            * |             .|0007|0008|REDACTED||
                            * |             .|0008|0013 0009|REDACTED||
                            |\ \        
                            | * |           .|0009|0010 0003|REDACTED||
                            | |\|       
                            | | *           .|0003|0004|REDACTED||
                            * | |           .|0013|0010|REDACTED||
                            |/ /        
                            * |             .|0010|0011|REDACTED||
                            * |             .|0011|0012|REDACTED||
                            * |             .|0012|0004|REDACTED||
                            |/          
                            *               .|0004|0005|REDACTED||
                            *               .|0005|0006|REDACTED||
                            *               .|0006|0014|REDACTED| (tag: v0.3.1)|
                            *               .|0014|0016|REDACTED||
                            *               .|0016|0017 0018|REDACTED||
                            |\          
                            | *             .|0018|0019|REDACTED||
                            | *             .|0019|0020|REDACTED||
                            | *             .|0020|0021|REDACTED||
                            | *             .|0021|0022|REDACTED||
                            | *             .|0022|0017|REDACTED||
                            |/          
                            *               .|0017|0023|REDACTED||
                            *               .|0023|0024 0025|REDACTED||
                            |\          
                            | *             .|0025|0026|REDACTED||
                            | *             .|0026|0027|REDACTED||
                            | *             .|0027|0028|REDACTED||
                            | *             .|0028|0029|REDACTED||
                            | *             .|0029|0030|REDACTED||
                            | *             .|0030|0031|REDACTED||
                            | *             .|0031|0032|REDACTED||
                            | *             .|0032|0033|REDACTED||
                            | *             .|0033|0034|REDACTED||
                            | *             .|0034|0035|REDACTED||
                            | *             .|0035|0036|REDACTED||
                            | *             .|0036|0037|REDACTED||
                            | *             .|0037|0038|REDACTED||
                            | *             .|0038|0039|REDACTED||
                            | *             .|0039|0040|REDACTED||
                            | *             .|0040|0041|REDACTED||
                            | *             .|0041|0042|REDACTED||
                            | *             .|0042|0043|REDACTED||
                            * |             .|0024|0043|REDACTED||
                            |/          
                            *               .|0043|0044|REDACTED||
                            *               .|0044|0045|REDACTED||
                            *               .|0045|0046|REDACTED||
                            *               .|0046|0047|REDACTED| (tag: v0.3.0)|
                            *               .|0047||REDACTED||
                            """);

    public LoggedScenario Scenario02 { get; } =
        new("0.1.0", "0002", """
                            *               .|0002|0001|REDACTED| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                            *               .|0001|0003|REDACTED||
                            *               .|0003|0004|REDACTED||
                            *               .|0004|0005|REDACTED||
                            *               .|0005|0006|REDACTED||
                            *               .|0006|0007|REDACTED||
                            *               .|0007|0008|REDACTED||
                            *               .|0008|0009|REDACTED||
                            *               .|0009|0010|REDACTED||
                            *               .|0010|0011 0012|REDACTED||
                            |\             
                            | *             .|0012|0014|REDACTED||
                            * |             .|0011|0014|REDACTED||
                            |/             
                            *               .|0014|0015|REDACTED||
                            *               .|0015|0016 0017|REDACTED||
                            |\             
                            | *             .|0017|0018|REDACTED||
                            | *             .|0018|0019|REDACTED||
                            | *             .|0019|0020|REDACTED||
                            | *             .|0020|0016|REDACTED||
                            |/             
                            *               .|0016|0021 0022|REDACTED||
                            |\             
                            | *             .|0022|0023|REDACTED||
                            | *             .|0023|0021|REDACTED||
                            |/             
                            *               .|0021|0024|REDACTED||
                            *               .|0024|0025 0026|REDACTED||
                            |\             
                            | *             .|0026|0027|REDACTED||
                            * |             .|0025|0027 0028|REDACTED||
                            |\ \           
                            | |/           
                            |/|            
                            | *             .|0028|0029|REDACTED||
                            | *             .|0029|0030|REDACTED||
                            | *             .|0030|0027|REDACTED||
                            |/             
                            *               .|0027||REDACTED||
                            """);

    public LoggedScenario Scenario03 { get; } =
            new("0.3.4", "0002", """
                                *               .|0002|0001|REDACTED| (HEAD -> REDACTED_BRANCH, tag: tag: v0.3.3, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                                *               .|0001|0003|REDACTED| (tag: v0.3.2)|
                                *               .|0003|0004|REDACTED||
                                *               .|0004|0005 0006|REDACTED||
                                |\          
                                | *             .|0006|0008|REDACTED||
                                * |             .|0005|0009|REDACTED||
                                * |             .|0009|0010 0011|REDACTED||
                                |\ \        
                                | * |           .|0011|0012 0008|REDACTED||
                                | |\|       
                                | | *           .|0008|0013|REDACTED||
                                * | |           .|0010|0012|REDACTED||
                                |/ /        
                                * |             .|0012|0014|REDACTED||
                                * |             .|0014|0015|REDACTED||
                                * |             .|0015|0013|REDACTED||
                                |/          
                                *               .|0013|0016|REDACTED||
                                *               .|0016|0017|REDACTED||
                                *               .|0017|0018|REDACTED| (tag: v0.3.1)|
                                *               .|0018|0019|REDACTED||
                                *               .|0019|0020 0021|REDACTED||
                                |\          
                                | *             .|0021|0022|REDACTED||
                                | *             .|0022|0023|REDACTED||
                                | *             .|0023|0024|REDACTED||
                                | *             .|0024|0025|REDACTED||
                                | *             .|0025|0020|REDACTED||
                                |/          
                                *               .|0020|0026|REDACTED||
                                *               .|0026|0027 0028|REDACTED||
                                |\          
                                | *             .|0028|0029|REDACTED||
                                | *             .|0029|0030|REDACTED||
                                | *             .|0030|0031|REDACTED||
                                | *             .|0031|0032|REDACTED||
                                | *             .|0032|0033|REDACTED||
                                | *             .|0033|0034|REDACTED||
                                | *             .|0034|0035|REDACTED||
                                | *             .|0035|0036|REDACTED||
                                | *             .|0036|0037|REDACTED||
                                | *             .|0037|0038|REDACTED||
                                | *             .|0038|0039|REDACTED||
                                | *             .|0039|0040|REDACTED||
                                | *             .|0040|0041|REDACTED||
                                | *             .|0041|0042|REDACTED||
                                | *             .|0042|0043|REDACTED||
                                | *             .|0043|0044|REDACTED||
                                | *             .|0044|0045|REDACTED||
                                | *             .|0045|0046|REDACTED||
                                * |             .|0027|0046|REDACTED||
                                |/          
                                *               .|0046|0047|REDACTED||
                                *               .|0047|0048|REDACTED||
                                *               .|0048|0049|REDACTED||
                                *               .|0049|0050|REDACTED| (tag: v0.3.0)|
                                *               .|0050||REDACTED||
                                """);

    public LoggedScenario Scenario04 { get; } =
            new("0.3.5", "0002", """
                                *               .|0002|0001|REDACTED| (HEAD -> REDACTED_BRANCH, tag: v0.3.4, tag: v0.3.3, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                                *               .|0001|0003|REDACTED| (tag: v0.3.2)|
                                *               .|0003|0004|REDACTED||
                                """);

    public LoggedScenario Scenario05 { get; } =
        new("0.3.5", "0002", """
                             *               .|0002|0001|REDACTED| (HEAD -> REDACTED_BRANCH, tag: v0.3.3, tag: v0.3.4, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                             *               .|0001|0003|REDACTED| (tag: v0.4.0)|
                             *               .|0003|0004|REDACTED||
                             """);
}
