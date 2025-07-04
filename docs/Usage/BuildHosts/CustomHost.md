﻿---
uid: custom-host
---

<div style="background-color:#944248;padding:0px;margin-bottom:0.5em">
  <img src="https://noetictools.github.io/Git2SemVer.MSBuild/Images/Git2SemVer_banner_840x70.png"/>
</div>

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MsBuild)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>

# Custom host

> [!NOTE]
> If youre using a build system not listed here (like Jenkins) and you would like in-built support, please raise a feature request.

## Detection

A custom host is only used when [Git2SemVer_Env_HostType](xref:msbuild-properties##inputs) property is set to to `Custom`.

## Build number

Set by MSBuild properties:

* [Git2SemVer_BuildNumber](xref:msbuild-properties##inputs)
* [Git2SemVer_BuildContext](xref:msbuild-properties##inputs)
* [Git2SemVer_BuildIDFormat](xref:msbuild-properties##inputs)

## Properties

The build host object's properties:

| Host property | Description  |
|:-- |:-- |
| Build number  | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildNumber](xref:msbuild-properties##inputs) property. |
| Build context | Default is 'UNKNOWN'. Set via MSBuild [Git2SemVer_BuildContext](xref:msbuild-properties##inputs) property. |
| Build ID      | `<build number>` |
| IsControlled          | true          |
| Name                  | 'GitHub'    |

## Services

| Service | Description  |
|:-- |:-- |
| BumpBuildNumber       | Not supported (does nothing) |
| ReportBuildStatistic  | Not supported (does nothing) |
| SetBuildLabel         | Not supported (does nothing) |

