﻿using Moq;
using NoeticTools.Git2SemVer.Framework.Generation;


// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.Framework.Tests.Generation.ProjectVersioningTests;

internal class SolutionClientProjectUnitTests : ProjectVersioningUnitTestsBase
{
    [SetUp]
    public void SetUp()
    {
        ModeIs(VersioningMode.SolutionClientProject);
        Host.Setup(x => x.BuildNumber).Returns("42");
    }

    [Test]
    public void DoesGenerate_WhenCachedOutputsNotAvailableTest()
    {
        LocalCachedOutputs.Setup(x => x.IsValid).Returns(false);
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(false);

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Once);
        Assert.That(result, Is.SameAs(GeneratedOutputs.Object));
    }

    [Test]
    public void DoesGenerate_WhenLocalCacheHasSameBuildNumberTest()
    {
        LocalCachedOutputs.Setup(x => x.IsValid).Returns(true);
        LocalCachedOutputs.Setup(x => x.BuildNumber).Returns("42");
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(false);

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Once);
        Assert.That(result, Is.SameAs(GeneratedOutputs.Object));
    }

    [Test]
    public void DoesGenerate_WhenNoLocalCacheButSharedCacheHasSameBuildNumberTest()
    {
        LocalCachedOutputs.Setup(x => x.IsValid).Returns(false);
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(true);
        SharedCachedOutputs.Setup(x => x.BuildNumber).Returns("42");

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Once);
        Assert.That(result, Is.SameAs(GeneratedOutputs.Object));
    }

    [Test]
    public void DoesNotGenerate_WhenLocalCacheHasDifferentBuildNumberTest()
    {
        LocalCachedOutputs.Setup(x => x.IsValid).Returns(true);
        LocalCachedOutputs.Setup(x => x.BuildNumber).Returns("41");
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(false);

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Never);
        Assert.That(result, Is.SameAs(SharedCachedOutputs.Object));
    }

    [Test]
    public void DoesNotGenerate_WhenNoLocalCacheAndSharedCacheHasDifferentBuildNumberTest()
    {
        LocalCachedOutputs.Setup(x => x.IsValid).Returns(false);
        SharedCachedOutputs.Setup(x => x.IsValid).Returns(true);
        SharedCachedOutputs.Setup(x => x.BuildNumber).Returns("43");

        var result = Target.Run();

        VersionGenerator.Verify(x => x.PrebuildRun(), Times.Never);
        Assert.That(result, Is.SameAs(SharedCachedOutputs.Object));
    }
}