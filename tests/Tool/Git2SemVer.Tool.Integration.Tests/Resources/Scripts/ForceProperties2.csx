using System;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.MSBuild;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using Semver;

var context = VersioningContext.Instance!;

context.Logger.LogInfo("\nRunning script ForceProperties2.csx.\n");

// force base version outputs & clear derived.
// Version will be dominant.
context.Outputs.Version = new SemVersion(21, 22, 23).WithPrerelease("beta");
context.Outputs.InformationalVersion = null;
context.Outputs.AssemblyVersion = null;
context.Outputs.FileVersion = null;
context.Outputs.PackageVersion = null;

/*
 * Expected outcome:
 *
 *  Assembly version:       21.22.23.0
 *  File version:           21.22.23.0
 *  Informational version:  21.22.23-beta
 *  Product version:        21.22.23-beta
 */