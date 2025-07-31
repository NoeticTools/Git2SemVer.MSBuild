﻿using NoeticTools.Git2SemVer.Core;
using NoeticTools.Git2SemVer.Framework.Generation;
using NoeticTools.Git2SemVer.Framework.Generation.Builders.Scripting;
using NoeticTools.Git2SemVer.IntegrationTests.Framework;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoeticTools.Git2SemVer.IntegrationTests;

internal class ScriptExecutionIntegrationTests : ScriptingTestsBase
{
    private const string TestScriptFilename = "TestScript.csx";
    private BuildEngine9Stub _buildEngine;
    private Dictionary<string, string> _globalProperties;
    private MSBuildGlobalProperties _msBuildGlobalProperties;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        OneTimeSetUpBase();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        OneTimeTearDownBase();
    }

    [SetUp]
    public void SetUp()
    {
        SetUpBase();

        _globalProperties = new Dictionary<string, string>();
        _buildEngine = new BuildEngine9Stub(_globalProperties);
        _msBuildGlobalProperties = new MSBuildGlobalProperties(_buildEngine);
    }

    [Test]
    [MaxTime(10000)]
    public void ControlledPrereleaseBuildScenario01()
    {
        var context = GetContext("12345", "1");
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, Git, context.Inputs, context.Outputs, _msBuildGlobalProperties);

        Assert.That(Logger.HasError, Is.False);
    }

    [Test]
    [MaxTime(10000)]
    public void UncontrolledPrereleaseBuildScenario01()
    {
        var context = GetContext("12345", "MACHINE-NAME");
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, Git, context.Inputs, context.Outputs, _msBuildGlobalProperties);

        Assert.That(Logger.HasError, Is.False);
    }

    [Test]
    [MaxTime(10000)]
    public void UncontrolledPrereleaseInitialDevBuildScenario01()
    {
        var context = GetContext("12345", "MACHINE-NAME");
        var runner = new ScriptVersionBuilder(Logger);

        runner.Build(context.Host, Git, context.Inputs, context.Outputs, _msBuildGlobalProperties);

        Assert.That(Logger.HasError, Is.False);
    }

    private VersioningContext GetContext(string hostBuildNumber,
                                         string hostBuildContext)
    {
        var taskInputs = GetTaskInputs();
        var buildHost = new BuildHostStub(Logger)
        {
            BuildNumber = hostBuildNumber,
            BuildContext = hostBuildContext
        };

        return new VersioningContext(taskInputs, new TaskOutputsStub(), buildHost, Git, _msBuildGlobalProperties, Logger);
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
}