---
uid: dotnet-tool-git2semver-tool
---

# Git2SemVer.Tool

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)

Git2SemVer.Tool is a dotnet tool used to setup a .NET solution for [solution versioning](xref:solution-versioning).

## Installing

To setup a solution to use Git2SemVer solution versioning, first install the dotnet tool `Git2SemVer.Tool`:

> [!NOTE]
> Git2SemVer.Tool is only required for solution setup, it is not required in the build environment.

```winbatch
dotnet tool install --global NoeticTools.Git2SemVer.Tool
```

Then, in the solution's directory, run:

```winbatch
Git2SemVer add
```

To update the tool to the latest:

```winbatch
dotnet tool update NoeticTools.Git2SemVer.Tool --global
```

You will be prompted with a few options and then the setup is done.
