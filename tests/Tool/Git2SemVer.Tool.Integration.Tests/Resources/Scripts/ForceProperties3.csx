using System;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.MSBuild;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using NoeticTools.Git2SemVer.MSBuild.Versioning.Generation.Builders.Scripting;
using Semver;

var context = VersioningContext.Instance!;

context.Logger.LogInfo("\nRunning script ForceProperties3.csx.\n");

// force base version outputs & clear derived.
// Version will be dominant.
context.Outputs.Version = new SemVersion(2, 2, 2).WithPrerelease("beta");
context.Outputs.InformationalVersion = null;
context.Outputs.AssemblyVersion = new Version(200, 201, 202);
context.Outputs.FileVersion = new Version(200, 201, 212);
context.Outputs.PackageVersion = new SemVersion(1, 2, 3).WithPrerelease("alpha");

/*
 * Expected outcome:
 *
 *  Assembly version:       200.201.202.0
 *  File version:           200.201.212
 *  Informational version:  2.2.2-beta
 *  Product version:        2.2.2-beta
 *  Pacakge version:        2.2.3-alpha
 */