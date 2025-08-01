﻿using Moq;
using NoeticTools.Git2SemVer.Framework.Versioning;


// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.Framework.Tests.Versioning.ProjectVersioningTests;

internal class SolutionVersionProjectUnitTests : ProjectVersioningUnitTestsBase
{
    [SetUp]
    public void SetUp()
    {
        ModeIs(VersioningMode.SolutionVersioningProject);
        SharedCachedOutputs.Setup(x => x.BuildNumber).Returns("42");
    }

    [Test]
    public void DoesGenerate_WhenCachedOutputsNotAvailableTest()
    {
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(false);

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Once);
        Assert.That(result.Versions, Is.SameAs(GeneratedOutputs.Object));
        OutputsCacheJsonFile.Verify(x => x.Load("IntermediateOutputDirectory"), Times.Never);
    }

    [Test]
    public void DoesNotGenerate_WhenCachedOutputsAvailableTest()
    {
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(true);

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Never);
        Assert.That(result.Versions, Is.SameAs(SharedCachedOutputs.Object));
        OutputsCacheJsonFile.Verify(x => x.Load("IntermediateOutputDirectory"), Times.Never);
    }
}