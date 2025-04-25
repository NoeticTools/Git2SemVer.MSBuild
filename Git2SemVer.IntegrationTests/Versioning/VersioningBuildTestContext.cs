using System.IO.Compression;
using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Core.Logging;
using NoeticTools.Git2SemVer.Core.Tools;
using NoeticTools.Git2SemVer.Core.Tools.DotnetCli;
using NoeticTools.Git2SemVer.Testing.Core;


#pragma warning disable NUnit2045

namespace NoeticTools.Git2SemVer.IntegrationTests.Versioning;

internal sealed class VersioningBuildTestContext : IDisposable
{
    private const int ConcurrentContextsLimit = 100;
    private static int _activeContexts;
    private readonly TestDirectoryResource _testDirectoryResource;
    private static int _contextIdCount;
    private readonly int _contextId;

    public VersioningBuildTestContext(string groupName, string solutionFolderName, string solutionFileName, string projectName)
    {
        if (_activeContexts > ConcurrentContextsLimit)
        {
            Assert.Fail($"Exceeded number of active contexts limit of {ConcurrentContextsLimit}.");
        }

        _activeContexts++;
        //TestContext.Out.WriteLine($"{_activeContexts} active contexts"); //>>>

        _contextId = _contextIdCount++;
        //TestContext.Out.WriteLine($"Context {_contextId} - Creating resources"); //>>>
        _testDirectoryResource = new TestDirectoryResource(groupName);

        Logger = new NUnitLogger(false) { Level = LoggingLevel.Trace };

        TestDirectory = _testDirectoryResource.Create();
        TestFolderName = TestDirectory.Name;
        Logger.LogInfo("Created test directory {0}.", TestDirectory.FullName);

        var processCli = new ProcessCli(Logger) { WorkingDirectory = TestDirectory.FullName };
        DotNetCli = new DotNetTool(processCli);

        var currentDirectory = Directory.GetCurrentDirectory();
        BuildConfiguration = new DirectoryInfo(currentDirectory).Parent!.Name;
        //_git2SemVerToolPath =
        //    Path.Combine(_solutionDirectory, "Git2SemVer.Tool/bin", BuildConfiguration, "net8.0", "NoeticTools.Git2SemVer.Tool.dll");


        var solutionDirectory = Path.Combine(TestDirectory.FullName, solutionFolderName);
        var projectPath = Path.Combine(solutionDirectory, projectName);
        TestSolutionPath = Path.Combine(solutionDirectory, solutionFileName);
        var testProjectBinDirectory = Path.Combine(projectPath, "bin", BuildConfiguration);
        CompiledAppPath = Path.Combine(testProjectBinDirectory, "net8.0", "NoeticTools.TestApplication.dll");
        PackageOutputDir = testProjectBinDirectory;
        ExtractResourceToDirectory(solutionFolderName + ".zip", TestDirectory.FullName);
    }

    public string BuildConfiguration { get; }

    public string CompiledAppPath { get; }

    public DotNetTool DotNetCli { get; }

    public NUnitLogger Logger { get; }

    public string PackageOutputDir { get; }

    public DirectoryInfo TestDirectory { get; }

    public string TestFolderName { get; }

    public string TestSolutionPath { get; }

    public static void AssertFileExists(string packageDirectory, string expectedFilename)
    {
        var directory = new DirectoryInfo(packageDirectory);
        var foundFiles = directory.GetFiles(expectedFilename);
        Assert.That(foundFiles.Length, Is.EqualTo(1), $"File '{expectedFilename}' does not exist.");
    }

    public string DeployScript(string scriptFilename)
    {
        var scriptPath = Path.Combine(TestDirectory.FullName, scriptFilename);
        GetType().Assembly.WriteResourceFile(scriptFilename, scriptPath);
        return scriptPath;
    }

    public void Dispose()
    {
        _activeContexts--;
        //TestContext.Out.WriteLine($"Context {_contextId} - Releasing resources");
        //TestContext.Out.Flush();
        //System.Threading.Thread.Sleep(100);//>>>
        _testDirectoryResource.Dispose();
        //TestContext.Out.WriteLine($"Context {_contextId} - Resources released");
    }

    public void DotNetCliBuildTestSolution(params string[] arguments)
    {
        //TestContext.Out.WriteLine("DotNetCliBuildTestSolution - 01");
        //TestContext.Out.Flush();

        var result = DotNetCli.Build(TestSolutionPath, BuildConfiguration, arguments);
        //TestContext.Out.WriteLine("DotNetCliBuildTestSolution - 02");
        //TestContext.Out.Flush();
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);
        Assert.That(Logger.HasError, Is.False);
    }

    public void PackTestSolution()
    {
        var result = DotNetCli.Pack(TestSolutionPath, BuildConfiguration, "--no-restore --no-build");
        Assert.That(result.returnCode, Is.EqualTo(0), result.stdOutput);
        Assert.That(Logger.HasError, Is.False);
    }

    private void ExtractResourceToDirectory(string filename, string extractPath)
    {
        if (Directory.Exists(extractPath))
        {
            Directory.Delete(extractPath, true);
            TestHelper.WaitUntil(() => !Directory.Exists(extractPath));
        }

        using var stream = GetType().Assembly.GetResourceStream(filename);
        ZipFile.ExtractToDirectory(stream, extractPath);
    }
}