---
uid: git2semver-tool-landing
---

# Git2SemVer.Tool

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)

**Git2SemVer.Tool** is a dotnet tool used to:

* Setup a .NET solution for [solution versioning](xref:solution-versioning).
* Run versioning engine to generate version information for diagnostics purposes. No project required.

> [!NOTE]
> If only using **Git2SemVer.Tool** for solution versioning setup, then it is not required in the build environment.

## Installing

To install the dotnet tool `Git2SemVer.Tool`:

```console
dotnet tool install --global NoeticTools.Git2SemVer.Tool
```

To update the tool to the latest:

```console
dotnet tool update NoeticTools.Git2SemVer.Tool --global
```

## Solution versioning

To setup all projects in a solution to use [solution versioning](xref:solution-versioning), in the solution's directory, run:

```console
Git2SemVer add
```

You will be prompted with a few options and then the setup is done.

## Run versioning engine

In the repository's directory run:

```console
Git2SemVer run
```
