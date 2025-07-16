---
uid: build-hosts
---

<div style="background-color:#944248;padding:0px;margin-bottom:0.5em">
  <img src="https://noetictools.github.io/Git2SemVer.MSBuild/Images/Git2SemVer_banner_840x70.png"/>
</div>

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>

# Build Hosts

A build host is the machine or software that is performing the build.

Example build hosts:

* A developer's box or virtual machine. The machine is the host.
* A build system software such as TeamCity or Jenkins.

If [Git2SemVer_Env_HostType](xref:versioning-msbuild-properties##inputs) is set, that build host type will be used.
Otherwise Git2SemVer will detect detectable hosts (currently TeamCity is the only detectable host) which host type to use.
If non found it will default to using the [uncontrolled host](xrf:uncontrolled-host) type.

## Types

Supported host types are:

* [TeamCity](xrf:teamcity)
* [GitHub workflows](xrf:github-workflows)
* [Custom host](xrf:custom-host)
* [Uncontrolled host](xrf:uncontrolled-host) - provides local build numbering

The build host object is available to the [C# script](xref:csharp-script) via the `BuildHost` property. The properties and services are described for each host type.

## Service calls

On every build Git2SemVer's default version generator calls services on the build host object.
If the build host does not support the service, it does nothing.

On every build default version generator will:

* Call `SetBuildLabel` with the generated build system version if [Git2SemVer_UpdateHostBuildLabel](xref:versioning-msbuild-properties##inputs) is set to true.
* Call `ReportBuildStatistic` with Git2SemVer's MSBuild task execution time.

The [C# script](xref:csharp-script) has access to these services via `BuildHost`.