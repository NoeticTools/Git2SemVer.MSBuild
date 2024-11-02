using System.Diagnostics;
using NoeticTools.Common.Logging;
using NoeticTools.Common.Tools;
using NoeticTools.Common.Tools.DotnetCli;
using NoeticTools.Testing.Common;


namespace NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;

public abstract class ScriptingTestsBase
{
    protected string TestFolderPath = "";

    protected void SetUpBase()
    {
        Logger = new NUnitTaskLogger { Level = LoggingLevel.Trace };
        TestFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                      "Git2SemVer",
                                      "TestData");
        if (Directory.Exists(TestFolderPath))
        {
            Directory.Delete(TestFolderPath, true);
            WaitUntil(() => !Directory.Exists(TestFolderPath));
        }

        Directory.CreateDirectory(TestFolderPath);
    }

    protected DotNetTool DotNetCli { get; private set; } = null!;

    protected ILogger Logger { get; private set; } = null!;

    protected static void WaitUntil(Func<bool> predicate)
    {
        var stopwatch = Stopwatch.StartNew();
        while (!predicate() && stopwatch.Elapsed < TimeSpan.FromSeconds(30))
        {
            Thread.Sleep(5);
        }
    }

    protected virtual void OneTimeSetUpBase()
    {
        Logger = new NUnitTaskLogger();
        DotNetCli = new DotNetTool(new ProcessCli(Logger));
    }
}