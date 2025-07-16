---
uid: changelog-gen-intro
---

<style>

.featureTitle {
  font-size:1.2em;
  font-weight:bold;
}

.iconcolumn {
  width:10%;
  text-align:center;
}

.featureBody {
  font-size:1.0em;
}

.featureBodyLeftAlign {
  font-size:1.0em;
  text-align:left;
}

table, tr {
  border:none !important;
}

td {
  border:none !important;
  width:300px;
}

a 
{
  text-decoration: none; 
}
</style>

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.Msbuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild)
[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)
[![Release Build](https://github.com/NoeticTools/Git2SemVer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NoeticTools/Git2SemVer/actions/workflows/dotnet.yml)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>


# Git2SemVer changelog generation

**Git2SemVer** is a Visual Studio and developer friendly <a href="https://semver.org">Semantic Versioning</a> and changelog generation framework for .NET solutions or projects using dotnet CLI or Visual Studio.
Every build, on both developer boxes and the build system, without scripts or environment tools. Just add the NuGet package.


**Git2SemVer** leverages [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) to silently version .NET solutions and projects.
The MSBuild Nuget packages adds **Git2SemVer** versioning to every build while the dotnet tool provides stand-alone use and changelog generation.


An example generated draft (pre-release) changelog fragment:

![](../Images/draft_changelog_fragment.png)
