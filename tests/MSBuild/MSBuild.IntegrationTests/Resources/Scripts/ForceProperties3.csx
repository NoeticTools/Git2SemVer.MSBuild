using System;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Framework;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;
using Semver;

var context = VersioningContext.Instance!;

context.Logger.LogInfo("Running script ForceProperties3.csx.");

// force base version outputs & clear derived.
// Version will be dominant.
context.Outputs.Version = new SemVersion(2, 2, 2).WithPrerelease("beta");
context.Outputs.InformationalVersion = null;
context.Outputs.AssemblyVersion = new Version(200, 201, 202);
context.Outputs.FileVersion = new Version(200, 201, 212);
context.Outputs.PackageVersion = new SemVersion(1, 2, 3).WithPrerelease("alpha");

// Note: Build system version not set to avoid chaning build number when running the tests
//context.Outputs.BuildSystemVersion = new SemVersion(1, 2, 3).WithPrerelease("alpha");

/*
 * Expected outcome:
 *
 *  Assembly version:       200.201.202.0
 *  File version:           200.201.212
 *  Informational version:  
 *  Product version:        2.2.2-beta
 *  Package version:        2.2.3-alpha
 */