using System;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.Framework;
using NoeticTools.Git2SemVer.Framework.Framework.Semver;
using NoeticTools.Git2SemVer.Framework.Versioning.Builders.Scripting;
using Semver;

var context = VersioningContext.Instance!;

context.Logger.LogInfo("Running script ForceProperties2.csx.");

// force base version outputs & clear derived.
// Version will be dominant.
context.Outputs.Version = new SemVersion(21, 22, 23).WithPrerelease("beta");
context.Outputs.InformationalVersion = null;
context.Outputs.AssemblyVersion = null;
context.Outputs.FileVersion = null;
context.Outputs.PackageVersion = null;

// Note: Build system version not set to avoid chaning build number when running the tests
//context.Outputs.BuildSystemVersion = new SemVersion(1, 2, 3).WithPrerelease("alpha");

/*
 * Expected outcome:
 *
 *  Assembly version:       21.22.23.0
 *  File version:           21.22.23.0
 *  Informational version:  21.22.23-beta
 *  Product version:        21.22.23-beta
 */