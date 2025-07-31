using System.Collections;
using NoeticTools.Git2SemVer.Core;


// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.Framework.Tests.Generation.GitHistoryWalking;

internal sealed class ScenariosFromBuildLogsTestSource : IEnumerable
{
    private const char ETX = CharacterConstants.ETX;
    private const char NL = CharacterConstants.GS; // manual replacement for new line
    private const char STX = CharacterConstants.STX;
    private const char US = CharacterConstants.US;

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
        new("0.3.3", "0002", $"""
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

    /// <summary>
    ///     Tests release tags on head.
    /// </summary>
    public LoggedScenario Scenario04 { get; } =
        new("0.3.4", "0002", $"""
                              *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: v0.3.4, tag: v0.3.3, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.3.2)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests release tags on head.
    /// </summary>
    public LoggedScenario Scenario05 { get; } =
        new("0.3.4", "0002", $"""
                              *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: v0.3.3, tag: v0.3.4, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v0.4.0)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests feature conventional commit.
    /// </summary>
    public LoggedScenario Scenario06 { get; } =
        new("1.3.0", "0002", $"""
                              *               {US}.|0002|0001|{STX}feat: add great feature{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v1.2.3)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests breaking change (!) conventional commit.
    /// </summary>
    public LoggedScenario Scenario07 { get; } =
        new("2.0.0", "0002", $"""
                              *               {US}.|0002|0001|{STX}feat!: add great feature{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v1.2.3)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests breaking change (footer) conventional commit.
    /// </summary>
    public LoggedScenario Scenario08 { get; } =
        new("2.0.0", "0002", $"""
                              *               {US}.|0002|0001|{STX}feat: add great feature{ETX}|{STX}{NL}BREAKING CHANGE: sorry{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: v1.2.3)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests tag takes precedence over conventional commits flags on a commit.
    /// </summary>
    public LoggedScenario Scenario09 { get; } =
        new("4.0.1", "0002", $"""
                              *               {US}.|0002|0001|{STX}fix: fix bug{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}feat: add great feature{ETX}|{STX}{ETX}| (tag: v4.0.0)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests that two fix bumps in segment only bumps patch once.
    /// </summary>
    public LoggedScenario Scenario10 { get; } =
        new("0.19.1", "0002", """
                              *               .|0002|0003|fix: Fix commit version when tag and conventional commit bump on same commit.|| (HEAD -> REDACTED_BRANCH, origin/main)|
                              *               .|0003|0004|fix: git2semver.msbuild unable to load|||
                              *               .|0004|0005|feat: version bump|| (tag: v0.19.0)|
                              *               .|0005|0006|REDACTED|| (tag: v0.18.2)|
                              *               .|0006|0007|REDACTED|||
                              """);

    /// <summary>
    ///     Tests release tag and major (!) bump on head.
    /// </summary>
    public LoggedScenario Scenario11 { get; } =
        new("1.0.0", "0002", $"""
                              *               {US}.|0002|0001|{STX}fix!: REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, tag: v1.0.0, origin/REDACTED_BRANCH, origin/REDACTED_BRANCH)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}||
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    /// <summary>
    ///     Tests conventional commits a path algorithm.
    /// </summary>
    /// <remarks>
    ///     Actual failure scenario from logs. Was returning 1.0.1 instead of 1.1.0.
    /// </remarks>
    public LoggedScenario Scenario12 { get; } =
        new("1.1.0", "0002", """
                             *               .|0002|0003|REDACTED|| (HEAD -> REDACTED_BRANCH, origin/Investigating)|
                             *               .|0003|0004 0005|REDACTED|| (origin/REDACTED_BRANCH, main)|
                             | *             .|0005|0006|REDACTED|| (origin/REDACTED_BRANCH, feature/cross-targeting)|
                             | *             .|0006|0007|refactor: REDACTED|||
                             | *             .|0007|0008|test: REDACTED|||
                             | *             .|0008|0009|test: REDACTED|||
                             | *             .|0009|0010|test: REDACTED|||
                             | *             .|0010|0011|refactor: REDACTED|||
                             | *             .|0011|0012|test: REDACTED|||
                             | *             .|0012|0013|build: REDACTED|||
                             | *             .|0013|0014|test: REDACTED|||
                             | *             .|0014|0015|test: REDACTED|||
                             | *             .|0015|0016|fix: REDACTED|||
                             | *             .|0016|0017|REDACTED|||
                             | *             .|0017|0018|REDACTED|||
                             | *             .|0018|0019|test: REDACTED|||
                             | *             .|0019|0020|build: REDACTED|||
                             | *             .|0020|0021|build: REDACTED|||
                             | *             .|0021|0022|REDACTED|||
                             | *             .|0022|0023|build: REDACTED|||
                             | *             .|0023|0004|build: REDACTED|||
                             |/  
                             *               .|0004|0024|REDACTED|||
                             *               .|0024|0025|REDACTED|||
                             *               .|0025|0026|feat: REDACTED|||
                             *               .|0026|0027|refactor: REDACTED|||
                             *               .|0027|0028|refactor: REDACTED|||
                             *               .|0028|0029|build: REDACTED|||
                             *               .|0029|0030|feat: REDACTED|||
                             *               .|0030|0031|build: REDACTED|||
                             *               .|0031|0032 0033|REDACTED|||
                             |\  
                             | *             .|0033|0034|build(tool): REDACTED|||
                             | *             .|0034|0035|build: REDACTED|||
                             | *             .|0035|0036|REDACTED|||
                             | *             .|0036|0037|REDACTED|||
                             * |             .|0032|0037|REDACTED|||
                             |/  
                             *               .|0037|0038|build: REDACTED|| (tag: v1.0.0)|
                             *               .|0038|0039|build: REDACTED|||
                             *               .|0039|0040|fix: REDACTED|||
                             *               .|0040|0041|docs!: REDACTED|||
                             *               .|0041|0042|fix: REDACTED|||
                             *               .|0042|0043|build: REDACTED|||
                             """);

    /// <summary>
    ///     Breaking change on head commit not bumping major version.
    /// </summary>
    public LoggedScenario Scenario13 { get; } =
        new("1.0.0", "0001", """
                             *               .|0001|0002|feat!: REDACTED|| (HEAD -> REDACTED_BRANCH, origin/Preparing-release)|
                             *               .|0002|0003|build: REDACTED|||
                             *               .|0003|0004|REDACTED|||
                             *               .|0004|0005|refactor: REDACTED|| (origin/REDACTED_BRANCH, main)|
                             *               .|0005|0006 0007|REDACTED|||
                             |\  
                             | *             .|0007|0008|REDACTED|||
                             * |             .|0006|0008|test: REDACTED|||
                             |/  
                             *               .|0008|0009 0010|REDACTED|||
                             |\  
                             | *             .|0010|0011|REDACTED|||
                             | *             .|0011|0012|REDACTED|||
                             * |             .|0009|0012|fix: REDACTED|||
                             |/  
                             *               .|0012|0013|build: REDACTED|||
                             *               .|0013|0014|build: REDACTED|||
                             *               .|0014|0015|REDACTED|||
                             *               .|0015|0016|REDACTED|||
                             *               .|0016|0017|build: REDACTED|||
                             *               .|0017|0018|build: REDACTED|||
                             *               .|0018|0019|build: REDACTED|||
                             *               .|0019|0020|build: REDACTED|||
                             *               .|0020||REDACTED|||
                             """);
    
    /// <summary>
    ///     Tests waypoint tag with feature bump.
    /// </summary>
    public LoggedScenario Scenario14 { get; } =
        new("1.3.0", "0002", $"""
                              *               {US}.|0002|0001|{STX}REDACTED{ETX}|{STX}{ETX}| (HEAD -> REDACTED_BRANCH, origin/main)|
                              *               {US}.|0001|0003|{STX}REDACTED{ETX}|{STX}{ETX}| (tag: .git2semver.waypoint(v1.2.3).feat)|
                              *               {US}.|0003|0004|{STX}REDACTED{ETX}|{STX}{ETX}||
                              *               {US}.|0004||{STX}REDACTED{ETX}|{STX}{ETX}||
                              """);

    public IEnumerator GetEnumerator()
    {
        yield return new object[] { "Scenario 01", Scenario01 };
        yield return new object[] { "Scenario 02", Scenario02 };
        yield return new object[] { "Scenario 03", Scenario03 };
        yield return new object[] { "Scenario 04", Scenario04 };
        yield return new object[] { "Scenario 05", Scenario05 };
        yield return new object[] { "Scenario 06 - feature", Scenario06 };
        yield return new object[] { "Scenario 07 - braking change (!)", Scenario07 };
        yield return new object[] { "Scenario 08 - breaking change", Scenario08 };
        yield return new object[] { "Scenario 09 - tag trumps bump on same commit", Scenario09 };
        yield return new object[] { "Scenario 10", Scenario10 };
        yield return new object[] { "Scenario 11", Scenario11 };
        yield return new object[] { "Scenario 12", Scenario12 };
        yield return new object[] { "Scenario 13", Scenario13 };
        yield return new object[] { "Scenario 14 - waypoint with feat", Scenario14 };
    }
}