---
_layout: landing
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


<div style="background-color:#944248;padding:0px;margin-bottom:0.5em">
  <img src="https://noetictools.github.io/Git2SemVer.MSBuild/Images/Git2SemVer_banner_840x70.png"/>
</div>

[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.MSBuild?label=Git2SemVer.Msbuild)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.MSBuild)
[![Current Version](https://img.shields.io/nuget/v/NoeticTools.Git2SemVer.Tool?label=Git2SemVer.Tool)](https://www.nuget.org/packages/NoeticTools.Git2SemVer.Tool)
[![Release Build](https://github.com/NoeticTools/Git2SemVer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NoeticTools/Git2SemVer/actions/workflows/dotnet.yml)
<a href="https://github.com/NoeticTools/Git2SemVer">
  ![Static Badge](https://img.shields.io/badge/GitHub%20project-944248?logo=github)
</a>


# Git2SemVer

**Git2SemVer** is a Visual Studio and developer friendly <a href="https://semver.org">Semantic Versioning</a> framework for .NET solution/project versioning and changelog generation.
It works the same with both Visual Studio and dotnet CLI builds. 
Every build, on both developer boxes and the build system, get traceable build numbering (no commit counting).


<div class="container mb-4">
    <div class="row row-cols-xs-2 row-cols-sm-2 row-cols-md-3 g-4">
        <div class="col">
            <div class="card" >
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="VersioningIntro.html">Automatic Versioning</a></p>
                    <p>Tutorials to add automatic Semmantic versioning, from <a href="https://www.conventionalcommits.org/en/v1.0.0/">Conventional Commits</a>, to your projects or solution.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" >
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="ChangelogGenerationIntro.html">Changelog Generation</a></p>
                    <p>Tutorials to add incremental draft changelog generation from <a href="https://www.conventionalcommits.org/en/v1.0.0/">Conventional Commits</a>.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card">
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="MSBuildIntro.html">MSBuild</a></p>
                    <p><b>Git2SemVer.MSBuild</b> tutorials and documentation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card">
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="DotnetToolIntro.html">Dotnet tool</a></p>
                    <p><b>Git2SemVer.Tool</b> tutorials and documentation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card">
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="Learn/solution-versioning.html">Solution Versioning</a></p>
                    <p>Learn about <b>Git2SemVer</b> solution versioning.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" >
                <div class="card-body" style="min-height: 170px; min-width: 250px">
                    <p class="fw-semibold"><a href="https://github.com/noetictools/git2semver/tree/main/src/CHANGELOG.md">Releases</a></p>
                    <p>Releases changelog.</p>
                </div>
            </div>
        </div>
    </div>
</div>

## Quick links

* [Getting Started](xref:getting-started)
* [Default Versioning](xref:versioning)
* Usage
  * [Workflow](xref:workflow)
  * [Release Tagging](xref:release-tagging)
  * [Branch naming](xref:branch-naming)
  * [Build Hosts](xref:build-hosts)
  * [C# Script](xref:csharp-script)

 
## License

**Git2SemVer** uses the [MIT license](https://choosealicense.com/licenses/mit/).


## Acknowledgments

This project uses the following tools and libraries. Many thanks to those who created and manage them.

* [Spectre.Console](https://github.com/spectreconsole/spectre.console)
* [Injectio](https://github.com/loresoft/Injectio)
* [JetBrains Annotations](https://www.jetbrains.com/help/resharper/Code_Analysis__Code_Annotations.html)
* [TeamCity.ServiceMessages](https://github.com/JetBrains/TeamCity.ServiceMessages)
* [Semver](https://www.nuget.org/packages/Semver) - files copied to create subset
* [NuGet.Versioning](https://www.nuget.org/packages/NuGet.Versioning)
* [NUnit](https://www.nuget.org/packages/NUnit)
* [Moq](https://github.com/devlooped/moq)
* [docfx](https://dotnet.github.io/docfx/)
* [JsonPeek](https://www.clarius.org/json/)
* <a href="https://www.flaticon.com/free-icons/brain" title="brain icons">Brain icons created by Freepik - Flaticon</a>
* <a href="https://www.flaticon.com/free-icons/consistent" title="consistent icons">Consistent icons created by Freepik - Flaticon</a>
* <a href="https://www.flaticon.com/free-icons/programmer" title="programmer icons">Programmer icons created by Flowicon - Flaticon</a>
