using NoeticTools.Common;
using NoeticTools.Common.Logging;
using NoeticTools.Git2SemVer.MSBuild.IntegrationTests.Framework;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using Task = System.Threading.Tasks.Task;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoeticTools.Git2SemVer.MSBuild.IntegrationTests;

[TestFixture]
public class ScriptExecutionIntegrationTests : ScriptingTestsBase
{
    private const string TestScriptFilename = "TestScript.csx";
    private BuildEngine9Stub _buildEngine;
    private Dictionary<string, string> _globalProperties;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        OneTimeSetUpBase();
    }

    [SetUp]
    public void SetUp()
    {
        SetUpBase();

        _globalProperties = new Dictionary<string, string>();
        _buildEngine = new BuildEngine9Stub(_globalProperties);
    }

    [Test]
    [MaxTime(10000)]
    public void ControlledPrereleaseBuildScenario01()
    {
        var context = GetContext("12345", "1", true);
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, context.Inputs, context.Outputs);

        Assert.That(Logger.HasError, Is.False);
    }

    [Test]
    [MaxTime(10000)]
    public void UncontrolledPrereleaseBuildScenario01()
    {
        var context = GetContext("12345", "MACHINE-NAME", false);
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, context.Inputs, context.Outputs);

        Assert.That(Logger.HasError, Is.False);
    }

    [Test]
    [MaxTime(10000)]
    public void UncontrolledPrereleaseInitialDevBuildScenario01()
    {
        var context = GetContext("12345", "MACHINE-NAME", false);
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, context.Inputs, context.Outputs);

        Assert.That(Logger.HasError, Is.False);
    }

    private VersioningContext GetContext(string hostBuildNumber,
                                         string hostBuildContext,
                                         bool isAControlledBuild)
    {
        var taskInputs = GetTaskInputs();
        var buildHost = new BuildHostStub(Logger)
        {
            IsControlled = isAControlledBuild,
            BuildNumber = hostBuildNumber,
            BuildContext = hostBuildContext
        };

        return new VersioningContext(taskInputs, new TaskOutputsStub(), buildHost, Logger);
    }

    private IVersionGeneratorInputs GetTaskInputs()
    {
        var inputs = new VersionGeneratorInputsStub
        {
            BuildScriptPath = Path.Combine(TestFolderPath, TestScriptFilename),
            BuildEngine9 = _buildEngine
        };
        GetType().Assembly.WriteResourceFile("Git2SemVer.csx", inputs.BuildScriptPath);
        return inputs;
    }

    private static async Task DumpLoggedErrors(ILogger logger)
    {
        if (!string.IsNullOrWhiteSpace(logger.Errors))
        {
            await TestContext.Out.WriteLineAsync("Errors recorded:");
            await TestContext.Out.WriteLineAsync(logger.Errors);
        }
    }
}