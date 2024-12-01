﻿using Moq;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation;


// ReSharper disable InconsistentNaming

namespace NoeticTools.Git2SemVer.MSBuild.Tests.Versioning.Generation.ProjectVersioningTests;

internal class StandAloneProjectUnitTests : ProjectVersioningUnitTestsBase
{
    [SetUp]
    public void SetUp()
    {
        ModeIs(VersioningMode.StandAloneProject);
    }

    [TestCase]
    public void AlwaysGeneratesVersionTest()
    {
        var result = Target.Run();

        VersionGenerator.Verify(x => x.Run(), Times.Once);
        Assert.That(result, Is.SameAs(GeneratedOutputs.Object));
        OutputsCacheJsonFile.Verify(x => x.Load(It.IsAny<string>()), Times.Never);
        OutputsCacheJsonFile.Verify(x => x.Write(It.IsAny<string>(), It.IsAny<IVersionOutputs>()), Times.Never);
    }
}