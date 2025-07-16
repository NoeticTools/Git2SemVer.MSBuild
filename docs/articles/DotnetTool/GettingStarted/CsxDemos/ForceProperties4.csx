// For demonstration of concepts only

// These using statement are not necessary but may help editing
using System;
using Microsoft.Build.Utilities;
using NoeticTools.Git2SemVer.MSBuild;
using NoeticTools.Git2SemVer.MSBuild.Framework.Semver;
using NoeticTools.Git2SemVer.MSBuild.Scripting;
using Semver;   // the `Semver` library has been loading for the script to use.

// The versioning context gives the script access to:
// - All inputs and outputs.
// - API for working with semmantic versions.
var context = VersioningContext.Instance!;
context.Logger.LogInfo("Running demo Git2SemVer customisation script.");

// Demo that generated version information is accessible here
var version = context.Inputs.Version;
var buildNumber = context.Host.BuildNumber;
var branchName = context.Outputs.Git.BranchName;
context.Logger.LogInfo($"""
                        Git2SemVer provided script information:
                          Version = ${version}
                          Build number = ${buildNumber} 
                        """);

// As a demonstration, force outputs, making each (assembly, file, package) different.
context.Outputs.Version = new SemVersion(11, 12, 13).WithPrerelease("a-prerelease");
context.Outputs.InformationalVersion = context.Outputs.Version.WithMetadata("metadata");
context.Outputs.AssemblyVersion = new Version(1, 2, 3);
context.Outputs.FileVersion = new Version(4, 5, 6);
context.Outputs.PackageVersion = new SemVersion(4, 6, 7);

/*
 * Expected outcome:
 *
 *  Assembly version:       1.2.3.0
 *  File version:           4.5.6
 *  Informational version:  11.12.13-a-prerelease+metadata
 *  Nuget package version   4.6.7
 */