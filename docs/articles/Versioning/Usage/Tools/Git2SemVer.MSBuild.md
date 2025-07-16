---
uid: git2semver-msbuild
---

# Git2SemVer.MSBuild

[![`Current Version`](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.Msbuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild)

Git2SemVer.MSBuild is a nuget package that adds MSBuild tasks to .NET C# projects to build version the project executable and package artefacts.

[Git2SemVer.Tool](xref:git2semver-tool-landing) configures all projects in a solution to use `Git2SemVer.MSBuild` and configures version sharing between the projects.

## Installing

First check [prerequisites](xref:git2semver-tool-prerequisites) and then:

* For [project versioning](xref:project-versioning) - Add the nuget package [NoeticTools.Git2SemVer.MSBuild](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild) to the project. 

* For [solution versioning](xref:solution-versioning) - Use [Git2SemVer.Tool](xref:git2semver-tool-landing). It will configure projects to use the NoeticTools.Git2SemVer.MSBuild package.

