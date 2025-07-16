---
uid: msbuild-task-intro
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


# Git2SemVer

**Git2SemVer** is a Visual Studio and developer friendly <a href="https://semver.org">Semantic Versioning</a> and changelog generation framework for .NET solutions or projects using dotnet CLI or Visual Studio.
Every build, on both developer boxes and the build system, without scripts or environment tools. Just add the NuGet package.

<!--
<div class="container-fluid mb-4">
    <div class="row row-cols-xs-2 row-cols-sm-2 row-cols-md-3 g-4">
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px">
                <div class="card-body" >
                    <p class="fw-semibold"><a href="Versioning/VersioningIntro.html">Automatic Versioning</a></p>
                    <p>Tutorials to add automatic Semmantic versioning, from <a href="https://www.conventionalcommits.org/en/v1.0.0/">Conventional Commits</a>, to your projects or solution.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px" >
                <div class="card-body">
                    <p class="fw-semibold"><a href="ChangelogGenerationIntro.html">Changelog Generation</a></p>
                    <p>Tutorials to add incremental draft changelog generation from <a href="https://www.conventionalcommits.org/en/v1.0.0/">Conventional Commits</a>.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px">
                <div class="card-body">
                    <p class="fw-semibold"><a href="MSBuildIntro.html">MSBuild</a></p>
                    <p><b>Git2SemVer.MSBuild</b> tutorials and documentation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px">
                <div class="card-body">
                    <p class="fw-semibold"><a href="DotnetTool/DotnetToolIntro.html">Dotnet tool</a></p>
                    <p><b>Git2SemVer.Tool</b> tutorials and documentation.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px">
                <div class="card-body">
                    <p class="fw-semibold"><a href="Learn/solution-versioning.html">Solution Versioning</a></p>
                    <p>Learn about <b>Git2SemVer</b> solution versioning.</p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card" style="min-height: 170px; min-width: 225px">
                <div class="card-body">
                    <p class="fw-semibold"><a href="https://github.com/noetictools/git2semver/tree/main/src/CHANGELOG.md">Releases</a></p>
                    <p>Releases changelog.</p>
                </div>
            </div>
        </div>
    </div>
</div>
-->

**Git2SemVer** leverages [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) to silently version .NET solutions and projects.
The MSBuild Nuget packages adds **Git2SemVer** versioning to every build while the dotnet tool provides stand-alone use and changelog generation.

<br/>

An example git workflow from a release `1.2.3` to the next release `2.0.0`:

```mermaid
gitGraph
        commit id:"1.2.3+99"
        
        commit id:"1.2.3+100" tag:"1.2.3"
        branch feature/berry order: 1
        checkout feature/berry
        commit id:"1.2.4-beta.101"

        checkout main
        commit id:"1.2.4+102"
        branch feature/peach order: 3
        checkout feature/berry

        branch develop/berry order: 2
        checkout develop/berry
        commit id:"feat:berry 1.3.0-alpha.103"
        checkout feature/berry
        merge develop/berry id:"1.3.0-beta.104"
        checkout main
        merge feature/berry id:"1.3.0+105"

        checkout feature/peach
        commit id:"fix:bug1 1.2.4-beta.106"
        commit id:"feat!:peach 2.0.0-beta.107"
        checkout main
        merge feature/peach id:"2.0.0+108" tag:"v2.0.0"

        commit id:"2.0.1-beta.108"
```
<br/>

An example generated draft (pre-release) changelog fragment:

![](../Images/draft_changelog_fragment.png)


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
