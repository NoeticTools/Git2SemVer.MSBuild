---
uid: git2semver-msbuild-installing
---

# Installing Git2SemVer.MSBuild

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.MSBuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild)

#  Prerequisites

**Git2SemVer.MSBuild** known compatibility:

* `dotnet.exe` `8.0.403` or later.
* Windows 11
* Ubuntu 20.04 LTS

## Installing

### Project versioning

For [project versioning](xref:versioning-project-versioning), Add the nuget package [NoeticTools.Git2SemVer.MSBuild](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild) to the project. 

```console
dotnet nuget add NoeticTools.Git2SemVer.MSBuild
```

### Solution versioning

For [solution versioning](xref:versioning-solution-versioning) - Use [Git2SemVer.Tool](xref:git2semver-tool-landing) to setup the solution. 
It will configure the projects to use the [NoeticTools.Git2SemVer.MSBuild](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild) package.