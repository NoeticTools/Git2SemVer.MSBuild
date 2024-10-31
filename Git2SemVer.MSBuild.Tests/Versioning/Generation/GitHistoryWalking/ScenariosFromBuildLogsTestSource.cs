using System.Collections;
using NoeticTools.Common;
// ReSharper disable InconsistentNaming


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

    private const string STX = ControlCharacterConstants.STX;
    private const string ETX = ControlCharacterConstants.ETX;
    private const string US = ControlCharacterConstants.US;

    public LoggedScenario Scenario01 { get; } =
        new("0.3.2", "0001", $"""
                            *               {US}.|0001|0007 0002|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                            |\          
                            | *             {US}.|0002|0003|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0007|0008|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0008|0013 0009|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\ \        
                            | * |           {US}.|0009|0010 0003|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | |\|       
                            | | *           {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * | |           {US}.|0013|0010|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/ /        
                            * |             {US}.|0010|0011|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0011|0012|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0012|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/          
                            *               {US}.|0004|0005|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0005|0006|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0006|0014|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.1)|
                            *               {US}.|0014|0016|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0016|0017 0018|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\          
                            | *             {US}.|0018|0019|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0019|0020|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0020|0021|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0021|0022|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0022|0017|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/          
                            *               {US}.|0017|0023|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0023|0024 0025|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\          
                            | *             {US}.|0025|0026|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0026|0027|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0027|0028|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0028|0029|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0029|0030|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0030|0031|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0031|0032|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0032|0033|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0033|0034|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0034|0035|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0035|0036|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0036|0037|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0037|0038|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0038|0039|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0039|0040|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0040|0041|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0041|0042|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0042|0043|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0024|0043|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/          
                            *               {US}.|0043|0044|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0044|0045|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0045|0046|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0046|0047|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.0)|
                            *               {US}.|0047||{STX}REDACTED{ETX}|{STX}{ETX}||
                            """);

    public LoggedScenario Scenario02 { get; } =
        new("0.1.0", "0002", $"""
                            *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                            *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0004|0005|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0005|0006|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0006|0007|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0007|0008|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0008|0009|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0009|0010|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0010|0011 0012|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\             
                            | *             {US}.|0012|0014|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0011|0014|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/             
                            *               {US}.|0014|0015|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0015|0016 0017|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\             
                            | *             {US}.|0017|0018|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0018|0019|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0019|0020|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0020|0016|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/             
                            *               {US}.|0016|0021 0022|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\             
                            | *             {US}.|0022|0023|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0023|0021|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/             
                            *               {US}.|0021|0024|{STX}REDACTED{ETX}|{STX}{ETX}||
                            *               {US}.|0024|0025 0026|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\             
                            | *             {US}.|0026|0027|{STX}REDACTED{ETX}|{STX}{ETX}||
                            * |             {US}.|0025|0027 0028|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |\ \           
                            | |/           
                            |/|            
                            | *             {US}.|0028|0029|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0029|0030|{STX}REDACTED{ETX}|{STX}{ETX}||
                            | *             {US}.|0030|0027|{STX}REDACTED{ETX}|{STX}{ETX}||
                            |/             
                            *               {US}.|0027||{STX}REDACTED{ETX}|{STX}{ETX}||
                            """);

    public LoggedScenario Scenario03 { get; } =
            new("0.3.4", "0002", $"""
                                *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: tag: v0.3.3, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                                *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.2)|
                                *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0004|0005 0006|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |\          
                                | *             {US}.|0006|0008|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * |             {US}.|0005|0009|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * |             {US}.|0009|0010 0011|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |\ \        
                                | * |           {US}.|0011|0012 0008|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | |\|       
                                | | *           {US}.|0008|0013|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * | |           {US}.|0010|0012|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |/ /        
                                * |             {US}.|0012|0014|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * |             {US}.|0014|0015|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * |             {US}.|0015|0013|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |/          
                                *               {US}.|0013|0016|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0016|0017|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0017|0018|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.1)|
                                *               {US}.|0018|0019|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0019|0020 0021|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |\          
                                | *             {US}.|0021|0022|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0022|0023|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0023|0024|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0024|0025|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0025|0020|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |/          
                                *               {US}.|0020|0026|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0026|0027 0028|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |\          
                                | *             {US}.|0028|0029|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0029|0030|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0030|0031|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0031|0032|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0032|0033|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0033|0034|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0034|0035|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0035|0036|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0036|0037|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0037|0038|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0038|0039|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0039|0040|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0040|0041|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0041|0042|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0042|0043|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0043|0044|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0044|0045|{STX}REDACTED{ETX}|{STX}{ETX}||
                                | *             {US}.|0045|0046|{STX}REDACTED{ETX}|{STX}{ETX}||
                                * |             {US}.|0027|0046|{STX}REDACTED{ETX}|{STX}{ETX}||
                                |/          
                                *               {US}.|0046|0047|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0047|0048|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0048|0049|{STX}REDACTED{ETX}|{STX}{ETX}||
                                *               {US}.|0049|0050|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.0)|
                                *               {US}.|0050||{STX}REDACTED{ETX}|{STX}{ETX}||
                                """);

    public LoggedScenario Scenario04 { get; } =
            new("0.3.5", "0002", $"""
                                *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: v0.3.4, tag: v0.3.3, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                                *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.2)|
                                *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                                """);

    public LoggedScenario Scenario05 { get; } =
        new("0.3.5", "0002", $"""
                             *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: v0.3.3, tag: v0.3.4, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                             *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.4.0)|
                             *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                             """);
}
