﻿namespace NoeticTools.Git2SemVer.IntegrationTests;

[TestFixture]
internal class CrossTargetingSolutionVersioningBuildTests : VersioningBuildTestsBase
{
    protected override string SolutionFolderName => "CrossTargetingVersioning";

    protected override string SolutionName => "CrossTargetingVersioning.sln";
}